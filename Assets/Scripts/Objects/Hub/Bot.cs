using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the script for Ray's AI bots. They can be deactivated after playing through Ray's level.
/// The script is applied to the bots in the AI server room.
/// </summary>

public class Bot : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The button that unlocks this power supply when pressed.")]
    private RingPuzzleButton button;

    private void OnEnable()
    {
        // Allow player to grab bot power if they have finished Ray's level.
        // We are not unsubscribing from this, just in case the scene change prevents re-subscribing.
        button.ButtonPressed += UnblockPowerSwitchAndEnableAudioSources;
    }

    /// <summary>
    /// When the player finishes the Ray level, we are unblocking the bot's power switch.
    /// We are also enabling its audio sources, since we disabled those earlier as a workaround for a bug.
    /// </summary>
    /// <param name="crewmember"></param>
    private void UnblockPowerSwitchAndEnableAudioSources()
    {
        UnblockChildPowerSwitch();
    }

    /// <summary>
    /// The player can now extract power from the power switch.
    /// </summary>
    private void UnblockChildPowerSwitch()
    {
        if (GetComponentInChildren<PowerSwitch>() != null)
            GetComponentInChildren<PowerSwitch>().UnblockPowerSwitch();
    }
}
