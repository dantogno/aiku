using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class changes the nav equipment's blocked audio clip after the hologenerator shuts down (so that it's no longer a Plum voice line, because she's deactivated).
/// The script is applied to the nav equipment's power switch.
/// </summary>

public class ChangePowerSwitchBlockedSound : PowerSwitch
{
    [SerializeField, Tooltip("This is the hologenerator shutdown button that triggers the AudioClip swap.")]
    private RingPuzzleButton button;

    [SerializeField, Tooltip("This is the new audio clip we want to play - a normal access denied sound, like all the other switches.")]
    private AudioClip newBlockedClip;

    protected override void OnEnable()
    {
        base.OnEnable();
        button.ButtonPressed += ChangeBlockedClip;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        button.ButtonPressed -= ChangeBlockedClip;
    }

    private void ChangeBlockedClip()
    {
        blockedAudioClip = newBlockedClip;
    }
}
