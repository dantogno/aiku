using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class allows the player to interact with the GMD only after visiting the Trevor level.
/// The script is applied to a "dummy" GMD, which is simply the GMD model, without all its scripts and features.
/// </summary>

public class FakeGMD : MonoBehaviour
{
    [SerializeField, Tooltip("The real GMD, with all associated functionality.")]
    private GameObject gmd;

    private void OnEnable()
    {
        HubSceneChanger.FinishedLevel += ActivateRealGMDAndDeactivateFakeGMD;
    }
    private void OnDisable()
    {
        HubSceneChanger.FinishedLevel -= ActivateRealGMDAndDeactivateFakeGMD;
    }

    /// <summary>
    /// Activate the real GMD, and deactivate this gameObject (the fake GMD).
    /// </summary>
    /// <param name="crewmember"></param>
    private void ActivateRealGMDAndDeactivateFakeGMD(HubSceneChanger.CrewmemberName crewmember)
    {
        if (gmd != null && crewmember == HubSceneChanger.CrewmemberName.Trevor)
        {
            gmd.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
