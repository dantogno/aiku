using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates the dialogue that instructs the player how to use the scope
/// </summary>
public class ActivateScopeInstructions : MonoBehaviour
{
    private int timesVOFinished = 0;

    private void OnEnable()
    {
        VOAudio.VOAudioFinished += IncrementTimesVOFinished;
    }

    private void OnDisable()
    {
        VOAudio.VOAudioFinished -= IncrementTimesVOFinished;
    }

    /// <summary>
    /// Increments every time a VO has finished. When the right amount has finished, activate the instructions.
    /// </summary>
    private void IncrementTimesVOFinished()
    {
        timesVOFinished++;

        if (timesVOFinished >= 2)
        {
            ActivateInstructions();
        }
    }

    /// <summary>
    /// Activates the instruction dialogue
    /// </summary>
    private void ActivateInstructions()
    {
        GetComponent<VOAudio>().TriggerVOAudio();
        this.enabled = false;
    }
}
