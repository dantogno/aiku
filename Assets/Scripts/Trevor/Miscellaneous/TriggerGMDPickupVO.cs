using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Triggers the VO that plays once the player has picked up the GMD
/// </summary>
public class TriggerGMDPickupVO : MonoBehaviour
{
    private void OnEnable()
    {
        PickupGMD.PickedUpGMD += TriggerPickupVO;
    }

    private void OnDisable()
    {
        PickupGMD.PickedUpGMD -= TriggerPickupVO;
    }

    /// <summary>
    /// Triggers audio, disables script
    /// </summary>
    private void TriggerPickupVO()
    {
        GetComponent<VOAudio>().TriggerVOAudio();
        this.enabled = false;
    }
}
