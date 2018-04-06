using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This power switch is specialized, and must be applied to the power switch for the AI cooling system.
/// </summary>

public class CoolingSystemPowerSwitch : PowerSwitch
{
    [SerializeField, Tooltip("All of the audio sources for bots in the AI server room.")]
    private AudioSource[] botVoices;

    protected override void TransferPower(IPowerable otherPowerable)
    {
        base.TransferPower(otherPowerable);

        // When the player tries to transfer power from the cooling systems, all the bots have something to say about it.
        foreach (AudioSource a in botVoices) a.Play();
    }
}
