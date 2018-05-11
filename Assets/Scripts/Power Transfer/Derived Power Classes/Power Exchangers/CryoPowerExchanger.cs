using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The power exchangers for cryochambers are specialized.
/// Unlike power switches, cryochamber power exchangers operate incrementally, rather than all-or-nothing.
/// </summary>

public class CryoPowerExchanger : PowerExchanger
{
    [Tooltip("Sound that plays when a power exchanger is activated.")]
    [SerializeField]
    private AudioClip powerOnAudioClip;

    [Tooltip("Sound that plays when a power exchanger is deactivated.")]
    [SerializeField]
    private AudioClip powerOffAudioClip;

    // The audio source component attached to the power exchanger, used to play sounds when the player interacts with the switch.
    private AudioSource myAudioSource;

    // Pitch changes a little bit each interaction, for variety.
    private float originalPitch;

    private bool collectedAllPower = false;

    protected override void Awake()
    {
        base.Awake();

        myAudioSource = GetComponent<AudioSource>();
        originalPitch = myAudioSource.pitch;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        EndingScreen.AllocatedAllShipboardPowerToCryochambers += OnCollectedAllPower;
    }
    protected override void OnDisable()
    {
        base.OnDisable();

        EndingScreen.AllocatedAllShipboardPowerToCryochambers -= OnCollectedAllPower;
    }

    /// <summary>
    /// Transfer power between an interacting agent (otherPowerable) and the power exchanger's connected powerable.
    /// </summary>
    /// <param name="otherPowerable"></param>
    protected override void TransferPower(IPowerable otherPowerable)
    {
        bool otherPowerableHasPower = otherPowerable.CurrentPower > 0,
            cryochamberHasRoomForMorePower = connectedPowerable.CurrentPower < connectedPowerable.RequiredPower;

        // If the other powerable can deposit power into the connected powerable...
        if (otherPowerableHasPower && cryochamberHasRoomForMorePower)
        {
            ReceivePowerFromOtherPowerable(otherPowerable);
        }

        // Otherwise, the other powerable can withdraw power from the connected powerable.
        else
        {
            AllowOtherPowerableToExtractPowerFromConnectedPowerable(otherPowerable);
        }

        // Play the appropriate sound effect for the power exchange.
        myAudioSource.Play();

        if (!collectedAllPower) FindObjectOfType<EndingScreen>().CheckScene();
    }

    /// <summary>
    /// Check how much power the connected powerable can accept from otherPowerable.
    /// Add that amount of power to the connected powerable and subtract it from otherPowerable.
    /// </summary>
    /// <param name="otherPowerable"></param>
    private void ReceivePowerFromOtherPowerable(IPowerable otherPowerable)
    {
        bool canAcceptPower =
            connectedPowerable.CurrentPower + otherPowerable.CurrentPower <= connectedPowerable.RequiredPower;

        if (canAcceptPower)
        {
            // Add otherPowerable.CurrentPower to connected powerable first, because otherPowerable.CurrentPower will change.
            connectedPowerable.AddPower(otherPowerable.CurrentPower);

            // Deplete otherPowerable.CurrentPower after adding it to the connected powerable.
            otherPowerable.SubtractPower(otherPowerable.CurrentPower);
        }
        else
        {
            // Deplete otherPowerable.CurrentPower first because connected powerable's power differential will change.
            otherPowerable.SubtractPower(connectedPowerable.RequiredPower - connectedPowerable.CurrentPower);

            // Add connected powerable's power differential to connected powerable after subtracting it from otherPowerable.CurrentPower.
            connectedPowerable.AddPower(connectedPowerable.RequiredPower - connectedPowerable.CurrentPower);
        }

        // The next sound to play should be the power-on clip, since the connected powerable is powering on.
        myAudioSource.clip = powerOnAudioClip;

        // Random pitching for variety.
        myAudioSource.pitch = UnityEngine.Random.Range(originalPitch - .1f, originalPitch + .1f);
    }

    /// <summary>
    /// Check how much power otherPowerable can withdraw from the connected powerable.
    /// Subtract that amount of power from the connected powerable and add it to otherPowerable.
    /// </summary>
    /// <param name="otherPowerable"></param>
    private void AllowOtherPowerableToExtractPowerFromConnectedPowerable(IPowerable otherPowerable)
    {
        bool hasMorePowerThanOtherPowerableCanHold =
            connectedPowerable.CurrentPower > otherPowerable.RequiredPower - otherPowerable.CurrentPower;

        if (hasMorePowerThanOtherPowerableCanHold)
        {
            // Deplete connectedPowerable.CurrentPower first because other powerable's power differential will change.
            connectedPowerable.SubtractPower(otherPowerable.RequiredPower - otherPowerable.CurrentPower);

            // Add other powerable's power differential to other powerable after subtracting it from connectedPowerable.CurrentPower.
            otherPowerable.AddPower(otherPowerable.RequiredPower - otherPowerable.CurrentPower);
        }
        else
        {
            // Add connectedPowerable.CurrentPower to other powerable first, because connectedPowerable.CurrentPower will change.
            otherPowerable.AddPower(connectedPowerable.CurrentPower);

            // Deplete connectedPowerable.CurrentPower after adding it to the other powerable.
            connectedPowerable.SubtractPower(connectedPowerable.CurrentPower);
        }

        // The next sound to play should be the power-off clip, since the connected powerable is powering off.
        myAudioSource.clip = powerOffAudioClip;

        // Random pitching for variety.
        myAudioSource.pitch = UnityEngine.Random.Range(originalPitch - .1f, originalPitch + .1f);
    }

    private void OnCollectedAllPower()
    {
        collectedAllPower = true;
    }
}
