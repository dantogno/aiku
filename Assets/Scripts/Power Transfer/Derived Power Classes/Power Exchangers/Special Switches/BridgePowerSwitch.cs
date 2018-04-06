using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This power switch is specialized, and must be applied to the power switch for the bridge equipment.
/// </summary>

public class BridgePowerSwitch : PowerSwitch
{
    [SerializeField, Tooltip("The audio source for Plum's voice, whose visualizer is attached to the bridge console.")]
    private AudioSource plumAudioSource;

    // Plum only speaks the first time the player interacts with the power switch.
    private bool hasPlayedLine = false;

    protected override void TransferPower(IPowerable otherPowerable)
    {
        base.TransferPower(otherPowerable);

        PlayPlumLine();
    }

    /// <summary>
    /// Plum notifies the player of their mission when the power switch is interacted with.
    /// </summary>
    private void PlayPlumLine()
    {
        if (!hasPlayedLine)
        {
            plumAudioSource.Play();
            hasPlayedLine = true;
        }
    }
}
