using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class changes the emissive lighting of the scene when the generator powers down.
/// It is a manager, and should have its own dedicated object which is easy to find in the hierarchy.
/// </summary>

public class HubLightingManager : MonoBehaviour
{
    [SerializeField, Tooltip("When the generator shuts off, these lights dim or turn off.")]
    private Renderer[] emissivesToDisableOnGeneratorShutdown;

    [SerializeField, Tooltip("When the generator shuts off, these lights turn on.")]
    private Renderer[] emissivesToEnableOnGeneratorShutdown;

    [SerializeField, Tooltip("When the generator shuts off, these lights change color.")]
    private Renderer[] emissivesWhichChangeColorOnGeneratorShutdown;

    [SerializeField, Tooltip("This is the color of the emergency lights.")]
    private Color emergencyLightColor = new Color(0.7019608f, 0f, 0.3294118f);

    [SerializeField, Tooltip("These floats represent the brightness of the different lights around the ship after the generator shuts down.")]
    private float disabledEmissiveIntensity = .5f, enabledEmissiveIntensity = 2, emergencyLightEmissiveIntensity = 5;

    private void OnEnable()
    {
        EngineSequenceManager.OnShutdown += SwitchToEmergencyLighting;
    }
    private void OnDisable()
    {
        EngineSequenceManager.OnShutdown -= SwitchToEmergencyLighting;
    }

    /// <summary>
    /// When the generator shuts down, dim some lights, brighten some lights, and change the color of some lights.
    /// </summary>
    private void SwitchToEmergencyLighting()
    {
        foreach (Renderer r in emissivesToDisableOnGeneratorShutdown)
        {
            // Change the color of the light's emission map to black, so that it looks like it has been turned off.
            r.material.SetColor("_EmissionColor", Color.black);

            // Change the scene's dynamic lighting so that it looks like the lights have been turned down.
            DynamicGI.SetEmissive(r, r.material.GetColor("_EmissionColor") * disabledEmissiveIntensity);
        }

        foreach (Renderer r in emissivesToEnableOnGeneratorShutdown)
        {
            // Change the scene's dynamic lighting so that it looks like the lights have been turned on.
            DynamicGI.SetEmissive(r, r.material.GetColor("_EmissionColor") * enabledEmissiveIntensity);
        }

        foreach (Renderer r in emissivesWhichChangeColorOnGeneratorShutdown)
        {
            // Change the color of the light's emission map, so that it isn't a different color than the light it is emitting.
            r.material.SetColor("_EmissionColor", emergencyLightColor);

            // Change the scene's dynamic lighting so that it looks like the lights are emitting a different color.
            DynamicGI.SetEmissive(r, emergencyLightColor * emergencyLightEmissiveIntensity);
        }
    }
}
