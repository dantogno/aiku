using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates the PickupGMD script after the first VOAudio has finished
/// </summary>
public class ManageGMDPickup : MonoBehaviour
{
    private int timesVOFinished = 0;
    private int timesSubtitleFinished = 0;

    private void OnEnable()
    {
        VOAudio.VOAudioFinished += IncrementTimesVOFinished;
        SubtitleManager.SubtitleFinished += IncrementTimesSubtitleFinished;
    }

    private void OnDisable()
    {
        VOAudio.VOAudioFinished -= IncrementTimesVOFinished;
        SubtitleManager.SubtitleFinished -= IncrementTimesSubtitleFinished;
    }

    /// <summary>
    /// Increments timesFinished by 1 every time a VOAudio finishes
    /// </summary>
    private void IncrementTimesVOFinished()
    {
        timesVOFinished++;

        if (timesVOFinished == 1)
        {
            TurnOnGMDPickupScript();
        }
    }

    /// <summary>
    /// Increments timesSubtitleFinished by 1 every time a Subtitle finishes
    /// </summary>
    /// <param name="i"></param>
    private void IncrementTimesSubtitleFinished(int i)
    {
        timesSubtitleFinished++;

        if (timesSubtitleFinished == 3)
        {
            TurnOnGMDScopeFunctionality();
        }
        else if (timesSubtitleFinished == 4)
        {
            TurnOnGMDScript();
        }
    }

    /// <summary>
    /// Turns on the PickupGMD script
    /// </summary>
    private void TurnOnGMDPickupScript()
    {
        if (GetComponent<PickupGMD>() != null)
            GetComponent<PickupGMD>().enabled = true;
    }

    /// <summary>
    /// Turns on the functionality of the GMD scope
    /// </summary>
    private void TurnOnGMDScopeFunctionality()
    {
        if (GetComponent<Scope>() != null)
            GetComponent<Scope>().enabled = true;
        if (GetComponent<Animator>() != null)
            GetComponent<Animator>().enabled = true;
    }

    /// <summary>
    /// Turns on the GMD script
    /// </summary>
    private void TurnOnGMDScript()
    {
        if(GetComponent<GMD>() != null)
            GetComponent<GMD>().enabled = true;
        this.enabled = false;
    }

}
