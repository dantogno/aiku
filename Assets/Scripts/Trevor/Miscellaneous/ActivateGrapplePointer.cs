using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates the pointer for the grapple object at the correct time when the player is able to grapple
/// </summary>
public class ActivateGrapplePointer : MonoBehaviour
{
    private int timesTeleported = 0;

    private void OnEnable()
    {
        PortalNew.PlayerTeleported += IncrementTimesTeleported;
    }

    private void OnDisable()
    {
        PortalNew.PlayerTeleported -= IncrementTimesTeleported;
    }

    /// <summary>
    /// Counts the number of times the player has teleported. If the number is greater than 3, activate the pointer.
    /// </summary>
    private void IncrementTimesTeleported()
    {
        timesTeleported++;

        if (timesTeleported >= 3)
        {
            ActivatePointer();
        }
    }

    /// <summary>
    /// Turns the pointer game object on. Disables this script.
    /// </summary>
    private void ActivatePointer()
    {
        if(this.gameObject.transform.GetChild(0).gameObject != null)
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        this.enabled = false;
    }
}
