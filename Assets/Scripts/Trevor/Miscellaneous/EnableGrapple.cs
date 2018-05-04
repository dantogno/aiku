using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enables grappling after the player has teleported enough times.
/// </summary>
public class EnableGrapple : MonoBehaviour
{
    private int timesTeleported;

    private void OnEnable()
    {
        PortalNew.PlayerTeleported += IncrementTimesTeleported;
    }

    private void OnDisable()
    {
        PortalNew.PlayerTeleported -= IncrementTimesTeleported;
    }

    /// <summary>
    /// Counts how many times the player has teleported and enables grappling once player has reached the target number of times.
    /// </summary>
    private void IncrementTimesTeleported()
    {
        timesTeleported++;

        if (timesTeleported == 3)
        {
            this.gameObject.tag = "Grapple";
        }
    }
}
