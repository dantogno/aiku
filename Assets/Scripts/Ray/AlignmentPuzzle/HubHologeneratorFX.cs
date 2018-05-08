using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class adds some player feedback to the hologenerator, in the form of visual and audio effects.
/// The script is applied to the Hub Hologenerator GameObject.
/// </summary>

public class HubHologeneratorFX : MonoBehaviour
{
    [SerializeField, Tooltip("The shutdown button on the hologenerator.")]
    private RingPuzzleButton button;

    [SerializeField, Tooltip("The AudioSource component which plays an error sound.")]
    private AudioSource errorSource;

    [SerializeField, Tooltip("The AudioSource component which plays a shutdown sound.")]
    private AudioSource shutdownSource;

    [SerializeField, Tooltip("The Renderer components whose emissives we want to disable on shutdown.")]
    private Renderer[] renderersToDisable;

    [SerializeField, Tooltip("The GameObjects we want to disable on shutdown.")]
    private GameObject[] gameObjectsToDeactivate;

    [SerializeField, Tooltip("The interactable scripts we want to disable on shutdown.")]
    private MonoBehaviour[] interactablesToDisable;

    [SerializeField, Tooltip("The color the emissives should flash when there is an error.")]
    private Color errorColor = Color.magenta;

    [SerializeField, Tooltip("The color the emissives should flash when there is an error.")]
    private Color errorAlbedoColor = Color.magenta;

    [SerializeField, Tooltip("How long the error color should last, in seconds.")]
    private float errorWaitTime = 1;

    [SerializeField, Tooltip("How long the shutdown should last, in seconds.")]
    private float shutdownWaitTime = 1;

    private void OnEnable()
    {
        HubRotateRings.TriedToSpinRings += OnTriedToSpinRings;
        button.ButtonPressed += OnButtonPressed;
    }
    private void OnDisable()
    {
        HubRotateRings.TriedToSpinRings -= OnTriedToSpinRings;
        button.ButtonPressed -= OnButtonPressed;
    }

    /// <summary>
    /// When the player tries to spin the rings before the puzzle is activated, turn the emissives red and play an error noise.
    /// </summary>
    private void OnTriedToSpinRings()
    {
        foreach (Renderer r in renderersToDisable) StartCoroutine(ShowErrorEmissionColor(r));

        if (errorSource != null) errorSource.Play();
    }

    /// <summary>
    /// When the player presses the shutdown button: play a sound, turn off emissives, and deacivate GameObjects.
    /// </summary>
    private void OnButtonPressed()
    {
        foreach (Renderer r in renderersToDisable) StartCoroutine(TurnOffEmissionAndLowerShutdownPitch(r));

        foreach (GameObject g in gameObjectsToDeactivate) g.SetActive(false);

        foreach (MonoBehaviour m in interactablesToDisable) m.enabled = false;

        shutdownSource.Play();
    }

    /// <summary>
    /// Turn emissives red, and then back to original color to show that the player shouldn't be using the hologenerator .
    /// </summary>
    private IEnumerator ShowErrorEmissionColor(Renderer r)
    {
        Color originalColor = r.material.GetColor("_EmissionColor"),
            originalAlbedoColor = r.material.color;

        // Change to error emissive color.
        r.material.SetColor("_EmissionColor", errorColor);

        // Change to error color.
        //r.material.color = errorAlbedoColor;

        // Wait.
        yield return new WaitForSeconds(errorWaitTime);

        // Change back to original emissive color.
        r.material.SetColor("_EmissionColor", originalColor);

        // Change back to original color.
        //r.material.color = originalAlbedoColor;
    }

    /// <summary>
    /// Turn off emission, to show that the player has deactivated the bots. Also lower the pitch of the shutdown AudioSource.
    /// </summary>
    private IEnumerator TurnOffEmissionAndLowerShutdownPitch(Renderer r)
    {
        Color originalColor = r.material.GetColor("_EmissionColor"),
                currentColor = originalColor;

        shutdownSource.pitch = .5f;

        float elapsedTime = 0;
        while (elapsedTime < shutdownWaitTime)
        {
            currentColor = Color.Lerp(originalColor, Color.black, elapsedTime / shutdownWaitTime);
            r.material.SetColor("_EmissionColor", currentColor);
            shutdownSource.pitch = Mathf.Lerp(.5f, 0, elapsedTime / shutdownWaitTime);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        r.material.SetColor("_EmissionColor", Color.black);
        shutdownSource.pitch = 0;
    }
}
