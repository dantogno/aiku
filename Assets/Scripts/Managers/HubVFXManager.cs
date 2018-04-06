using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls when the glitchy screen effects are applied to the player's camera.
/// Apply this script to a dedicated GameObject alongside any other managers, so that it is easy to find.
/// </summary>

public class HubVFXManager : MonoBehaviour
{
    // This event is broadcast when the player shuts down at the end of the game.
    public static event Action FinalVFXHasFinished;

    [SerializeField, Tooltip("One of the two glitch VFX scripts applied to the main camera.")]
    private GlitchEffect glitchEffect;

    [SerializeField, Tooltip("One of the two glitch VFX scripts applied to the main camera.")]
    private GlitchyEffect glitchEffect2;

    [SerializeField, Tooltip("This value controls how long the glitch effect lasts when the player powers on or enters levels.")]
    private float glitchTimer = 6;

    [SerializeField, Tooltip("This value controls how long the glitch effect lasts when the player powers off at the end of the game.")]
    private float powerOffTimer = 3;

    // These are the stored values for the glitch script variables. The names are misleading, because the values are manipulated each time the player enters a scene.
    private float originalDispIntensity, originalColorIntensity, originalJitter;

    private void OnEnable()
    {
        SceneChanger.SceneChangeStarted += CallGlitchCoroutine;
        SceneChanger.SceneChangeFinished += CallGlitchCoroutine;
        EndingScreen.TransferredPlayerPowerReserve += CallDeathCoroutine;
    }
    private void OnDisable()
    {
        SceneChanger.SceneChangeStarted -= CallGlitchCoroutine;
        SceneChanger.SceneChangeFinished -= CallGlitchCoroutine;
        EndingScreen.TransferredPlayerPowerReserve -= CallDeathCoroutine;
    }

    private void Start()
    {
        StartCoroutine(PlayTransitionalGlitchEffect());
    }

    /// <summary>
    /// Must be a void method, not an IEnumerator, to match other script's event signature.
    /// Otherwise we would just call PlayTransitionalGlitchEffect directly.
    /// </summary>
    private void CallGlitchCoroutine()
    {
        StartCoroutine(PlayTransitionalGlitchEffect());
    }

    /// <summary>
    /// Must be a void method, not an IEnumerator, to match other script's event signature.
    /// Otherwise we would just call PlayPowerDownGlitchEffect directly.
    /// </summary>
    private void CallDeathCoroutine()
    {
        StartCoroutine(PlayDeathGlitchEffect());
    }

    /// <summary>
    /// This is the glitch effect which plays during level transitions and at the beginning of the game, when the player powers on.
    /// Later, we might have separate coroutines for these two sequences, but for now the effect is identical.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayTransitionalGlitchEffect()
    {
        #region HACK. These "original" values are changed each time this coroutine is called, so the logic of this method is flawed. However, the desired effect is achieved, and we may implement an entirely different solution for level transitions in the future.

        originalDispIntensity = glitchEffect._DispIntensity;
        originalColorIntensity = glitchEffect._ColorIntensity;
        originalJitter = glitchEffect2._scanLineJitter;

        #endregion

        #region Gradually decrease glitchiness.

        float elapsedTime = 0;
        while (elapsedTime < glitchTimer)
        {
            glitchEffect._DispIntensity = Mathf.Lerp(originalDispIntensity, 0, elapsedTime / glitchTimer);
            glitchEffect._ColorIntensity = Mathf.Lerp(originalColorIntensity, 0, elapsedTime / glitchTimer);
            glitchEffect2._scanLineJitter = Mathf.Lerp(originalJitter, 0, elapsedTime / glitchTimer);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        // Turn off scan line jitter and turn the glitchEffect script off entirely.
        glitchEffect.enabled = false;
        glitchEffect2._scanLineJitter = 0;

        #endregion
    }

    /// <summary>
    /// This is the glitch effect which plays at the end of the game, when the player powers off.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayDeathGlitchEffect()
    {
        // Turn on glitch effect.
        glitchEffect.enabled = true;

        #region Gradually increase glitchiness.

        float elapsedTime = 0;
        while (elapsedTime < powerOffTimer)
        {
            glitchEffect._DispIntensity = Mathf.Lerp(0, 1, elapsedTime / powerOffTimer);
            glitchEffect._ColorIntensity = Mathf.Lerp(0, 1, elapsedTime / powerOffTimer);
            glitchEffect2._scanLineJitter = Mathf.Lerp(0, 1, elapsedTime / powerOffTimer);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        // Turn off scan line jitter and turn the glitchEffect script off entirely, to allow the player to see the end credits.
        glitchEffect.enabled = false;
        glitchEffect2._scanLineJitter = 0;

        #endregion

        // Invoke death event, to signal that the game has ended.
        if (FinalVFXHasFinished != null) FinalVFXHasFinished();
    }
}
