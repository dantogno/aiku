using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class changes the emissive lighting of the scene when the generator powers down.
/// It is a manager, and should have its own dedicated object which is easy to find in the hierarchy.
/// </summary>

public class HubLightingManager : MonoBehaviour
{
    [SerializeField, Tooltip("These are the lights we want to disable when the generator shuts down.")]
    private GameObject[] lightsToDisable;

    [SerializeField, Tooltip("These are the emergency lights we want to enable when the generator shuts down.")]
    private GameObject[] lightsToEnable;

    [SerializeField, Tooltip("These are the lights we want to dim when the generator shuts down.")]
    private GameObject[] lightsToDim;

    [SerializeField, Tooltip("This is the color of the emergency lights.")]
    private Color emergencyLightColor = new Color(0.7019608f, 0f, 0.3294118f);

    [SerializeField, Tooltip("These floats represent the brightness of the different lights around the ship after the generator shuts down.")]
    private float disabledEmissiveIntensity = .5f, enabledEmissiveIntensity = .5f, emergencyLightIntensity = 1.5f, dimmedLightIntensity = .75f;

    // Most walls have emissive strips. We want their emissive property to change dynamically during gameplay.
    private Renderer[] wallRenderers;

    private void Awake()
    {
        InitializeWallArray();
    }

    private void OnEnable()
    {
        EngineSequenceManager.OnShutdown += SwitchToEmergencyLighting;
    }
    private void OnDisable()
    {
        EngineSequenceManager.OnShutdown -= SwitchToEmergencyLighting;
    }

    private void Start()
    {
        DisableWallEmissives();

        //the title screen needs moody lighting
        if(FindObjectOfType<TitleScreenManager>() != null)
        {
            SwitchToEmergencyLighting();
        }
    }

    /// <summary>
    /// Find all walls in the scene and add their renderers to the array of wall renderers.
    /// </summary>
    private void InitializeWallArray()
    {
        // We are using FindGameObjectsWithTag because there are many walls scattered throughout the hierarchy, and we do not want to have to find and reference them all manually.
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

        // Initialize the wall renderer array to have the same number of elements as the array of walls.
        wallRenderers = new Renderer[walls.Length];

        // Each wall's renderer is added to the array.
        for (int i = 0; i < walls.Length; i++)
        {
            wallRenderers[i] = walls[i].GetComponent<Renderer>();
        }
    }

    /// <summary>
    /// At the beginning of the game, the walls are not emissive.
    /// </summary>
    private void DisableWallEmissives()
    {
        foreach (Renderer r in wallRenderers)
        {
            // Change the color of the light's emission map to black, so that it looks like it has been turned off.
            r.material.SetColor("_EmissionColor", Color.clear);

            // Change the scene's dynamic lighting so that it looks like the lights have been turned down.
            DynamicGI.SetEmissive(r, r.material.GetColor("_EmissionColor") * disabledEmissiveIntensity);
        }
    }

    /// <summary>
    /// After the generator shuts down, the walls become emissive.
    /// </summary>
    private void EnableWallEmissives()
    {
        foreach (Renderer r in wallRenderers)
        {
            // Change the color of the light's emission map to black, so that it looks like it has been turned off.
            r.material.SetColor("_EmissionColor", Color.white);

            // Change the scene's dynamic lighting so that it looks like the lights have been turned on.
            DynamicGI.SetEmissive(r, r.material.GetColor("_EmissionColor") * enabledEmissiveIntensity);
        }
    }

    /// <summary>
    /// When the generator shuts down, dim some lights and brighten some lights.
    /// </summary>
    private void SwitchToEmergencyLighting()
    {
        EnableWallEmissives();

        #region Disable non-emergency lights.

        foreach (GameObject g in lightsToDisable)
        {
            g.GetComponentInChildren<Light>().intensity = 0;
            g.GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", Color.black);
        }

        #endregion

        #region Enable emergency lights.

        foreach (GameObject g in lightsToEnable)
        {
            g.GetComponentInChildren<Light>().intensity = emergencyLightIntensity;
        }

        #endregion

        #region Dim some non-emergency lights to "cheat" and give the impression that lights are off, while still allowing the player to see where they are going.

        foreach (GameObject g in lightsToDim)
        {
            g.GetComponentInChildren<Light>().intensity = dimmedLightIntensity;
            g.GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", Color.black);
        }

        #endregion
    }
}
