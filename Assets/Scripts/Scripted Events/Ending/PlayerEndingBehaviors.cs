using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls the way the player behaves after losing half their power.
/// The script is applied to the player.
/// </summary>

public class PlayerEndingBehaviors : MonoBehaviour
{
    [SerializeField, Tooltip("Player's movement script.")]
    private CustomRigidbodyFPSController movementScript;

    [SerializeField, Tooltip("The main camera's glitch effect component.")]
    private GlitchEffect glitchEffect;

    private void OnEnable()
    {
        PlayerPowerable.TransferredPartOfPowerReserve += OnPowerDrained;
    }
    private void OnDisable()
    {
        PlayerPowerable.TransferredPartOfPowerReserve -= OnPowerDrained;
    }

    /// <summary>
    /// Slow down the player and cause their UI to become glitchy.
    /// </summary>
    private void OnPowerDrained()
    {
        movementScript.movementSettings.ForwardSpeed /= 2;
        movementScript.movementSettings.BackwardSpeed /= 2;
        movementScript.movementSettings.StrafeSpeed /= 2;

        glitchEffect.enabled = true;
        glitchEffect._DispIntensity = .05f;
    }

    /// <summary>
    /// Return the player's movement and UI to normal.
    /// </summary>
    private void OnPowerRestored()
    {
        movementScript.movementSettings.ForwardSpeed *= 2;
        movementScript.movementSettings.BackwardSpeed *= 2;
        movementScript.movementSettings.StrafeSpeed *= 2;

        glitchEffect.enabled = false;
        glitchEffect._DispIntensity = 0;
    }
}
