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

    [SerializeField, Tooltip("How many times slower the player should move at half-power.")]
    private float halfChargeSpeed = 4;

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
        movementScript.movementSettings.ForwardSpeed /= halfChargeSpeed;
        movementScript.movementSettings.BackwardSpeed /= halfChargeSpeed;
        movementScript.movementSettings.StrafeSpeed /= halfChargeSpeed;

        glitchEffect.enabled = true;
        glitchEffect._DispIntensity = .05f;
    }

    /// <summary>
    /// Return the player's movement and UI to normal.
    /// </summary>
    private void OnPowerRestored()
    {
        movementScript.movementSettings.ForwardSpeed *= halfChargeSpeed;
        movementScript.movementSettings.BackwardSpeed *= halfChargeSpeed;
        movementScript.movementSettings.StrafeSpeed *= halfChargeSpeed;

        glitchEffect.enabled = false;
        glitchEffect._DispIntensity = 0;
    }
}
