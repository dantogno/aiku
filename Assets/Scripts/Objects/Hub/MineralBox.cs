using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class allows the box of minerals to be picked up after the player has played through the Trevor level.
/// The script is applied to the box of minerals in Trevor's lab.
/// </summary>

public class MineralBox : MonoBehaviour
{
    private void OnEnable()
    {
        // We are not unsubscribing from this, just in case the scene change prevents re-subscribing.
        HubSceneChanger.FinishedLevel += ActivatePickUpScript;
    }
    private void OnDisable()
    {
        HubSceneChanger.FinishedLevel -= ActivatePickUpScript;
    }

    /// <summary>
    /// When the player has finished the Trevor level, they may pick up the minerals.
    /// </summary>
    private void ActivatePickUpScript(HubSceneChanger.CrewmemberName crewmember)
    {
        PickUp myPickUpComponent = GetComponent<PickUp>();

        if (crewmember == HubSceneChanger.CrewmemberName.Trevor) myPickUpComponent.enabled = true;
    }
}
