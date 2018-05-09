using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class enables and disables the green arrow above the mineral processor.
/// The script is applied to the tupperware container with minerals in it.
/// </summary>

public class MineralBoxPickup : PickUp
{
    [SerializeField, Tooltip("The canvas over the mineral processor.")]
    private GameObject greenArrowCanvas, myArrow;

    [SerializeField, Tooltip("The collider attached to the mineral processor game object.")]
    private Collider mineralProcessor;

    protected override void PickThisUp(GameObject agentInteracting)
    {
        base.PickThisUp(agentInteracting);

        // Set the green arrow active when the player picks up the minerals.
        greenArrowCanvas.SetActive(true);

        myArrow.SetActive(false);
    }

    /// <summary>
    /// HACK: Player cannot drop mineral box, because otherwise they cannot pick it back up.
    /// </summary>
    protected override void DropThis()
    {
        // Cannot drop.
    }

    protected void OnTriggerEnter(Collider other)
    {
        // Set the green arrow inactive if the minerals are dropped in the mineral processor and destroy minerals.
        if (other == mineralProcessor)
        {
            greenArrowCanvas.SetActive(false);

            Destroy(gameObject);
        }
    }
}
