using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enables and disables the first pointer arrow which guides the player to the GMD in the dropship
/// </summary>
public class ManageGMDArrow : MonoBehaviour
{
    private void OnEnable()
    {
        PickupGMD.PickedUpGMD += DisableArrow;
        VOAudio.VOAudioFinished += EnableArrow;
    }

    private void OnDisable()
    {
        PickupGMD.PickedUpGMD -= DisableArrow;
        VOAudio.VOAudioFinished -= EnableArrow;
    }

    /// <summary>
    /// Deactivates the arrow canvas and turns off the script
    /// </summary>
    private void DisableArrow()
    {
        this.gameObject.SetActive(false);
        this.enabled = false;
    }

    /// <summary>
    /// Turns the pointer arrow on once Norma's first dialogue has finished
    /// </summary>
    private void EnableArrow()
    {
        if(this.gameObject.transform.GetChild(0).gameObject != null)
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
}
