using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates Trevor's VO during the tableau
/// </summary>
public class ActivateTrevorTableauVO : MonoBehaviour
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
    /// Increments timesVOFinished every time a VO segment has finished. At the right amount of times, activate VO.
    /// </summary>
    private void IncrementTimesVOFinished()
    {
        timesVOFinished++;

        if (timesVOFinished == 3)
        {
            ActivateVO();
        }
    }

    /// <summary>
    /// Activates the VO Audio.
    /// </summary>
    private void ActivateVO()
    {
        GetComponent<VOAudio>().TriggerVOAudio();
    }
}
