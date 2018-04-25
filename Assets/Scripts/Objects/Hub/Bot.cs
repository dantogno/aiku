using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the script for Ray's AI bots. They can be deactivated after playing through Ray's level.
/// The script is applied to the bots in the AI server room.
/// </summary>

public class Bot : MonoBehaviour
{
    private void OnEnable()
    {
        // Allow player to grab bot power if they have finished Ray's level.
        // We are not unsubscribing from this, just in case the scene change prevents re-subscribing.
        HubSceneChanger.FinishedLevel += UnblockPowerSwitchAndEnableAudioSources;
    }

    /// <summary>
    /// When the player finishes the Ray level, we are unblocking the bot's power switch.
    /// We are also enabling its audio sources, since we disabled those earlier as a workaround for a bug.
    /// </summary>
    /// <param name="crewmember"></param>
    private void UnblockPowerSwitchAndEnableAudioSources(HubSceneChanger.CrewmemberName crewmember)
    {
        if (crewmember == HubSceneChanger.CrewmemberName.Ray)
        {
            UnblockChildPowerSwitch();
            EnableAudioSources();
        }
    }

    /// <summary>
    /// The player can now extract power from the power switch.
    /// </summary>
    private void UnblockChildPowerSwitch()
    {
        if (GetComponentInChildren<PowerSwitch>() != null)
            GetComponentInChildren<PowerSwitch>().UnblockPowerSwitch();
    }

    /// <summary>
    /// Enable all audio source components in the bot's children.
    /// </summary>
    private void EnableAudioSources()
    {
        foreach (AudioSource source in GetComponentsInChildren<AudioSource>())
            source.enabled = true;
    }
}
