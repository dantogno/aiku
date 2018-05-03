using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates the PickupGMD script after the first VOAudio has finished
/// </summary>
public class ManageGMDPickup : MonoBehaviour
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
    /// Increments timesFinished by 1 every time a VOAudio finishes
    /// </summary>
    private void IncrementTimesVOFinished()
    {
        timesVOFinished++;

        if (timesVOFinished >= 1)
        {
            TurnOnGMDPickupScript();
        }
    }

    /// <summary>
    /// Turns on the PickupGMD script. Disables this script.
    /// </summary>
    private void TurnOnGMDPickupScript()
    {
        if (GetComponent<PickupGMD>() != null)
            GetComponent<PickupGMD>().enabled = true;
        this.enabled = false;
    }
}
