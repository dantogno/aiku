using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class allows the reticle to be activated/deactivated alongside the GMD.
/// The script is applied to the GMD.
/// </summary>

[RequireComponent(typeof(GMD))]
public class ToggleGMDReticle : MonoBehaviour
{
    [SerializeField, Tooltip("This is the panel component which is being used as a reticle for the GMD.")]
    private Image reticle;

    [SerializeField, Tooltip("The collider blocking the lab.")]
    private Collider doorBlocker;

    private GMD gmd;

    private void OnEnable()
    {
        PickupGMD.PickedUpGMD += TurnOnReticle;
        TriggerForPlayerToDropGMD.DroppedGMD += TurnOffReticle;
    }

    private void OnDisable()
    {
        PickupGMD.PickedUpGMD -= TurnOnReticle;
        TriggerForPlayerToDropGMD.DroppedGMD -= TurnOffReticle;
    }

    private void Start()
    {
        gmd = GetComponent<GMD>();
    }
    
    /// <summary>
    /// Turn on the reticle when the player picks up the GMD.
    /// </summary>
    private void TurnOnReticle()
    {
        reticle.enabled = true;
        doorBlocker.enabled = false;
    }

    /// <summary>
    /// Turn off the reticle when the player drops the GMD.
    /// </summary>
    private void TurnOffReticle()
    {
        reticle.enabled = false;
    }
}
