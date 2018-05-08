using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class turns the GMD reticle on or off.
/// The script is applied to the GMD in the hub scene.
/// </summary>

public class ToggleReticle : MonoBehaviour
{
    [SerializeField, Tooltip("The reticle for the GMD.")]
    private Image reticle;

    private void OnEnable()
    {
        PickupGMD.PickedUpGMD += EnableReticle;
        TriggerForPlayerToDropGMD.DroppedGMD += DisableReticle;
    }
    private void OnDisable()
    {
        PickupGMD.PickedUpGMD -= EnableReticle;
        TriggerForPlayerToDropGMD.DroppedGMD -= DisableReticle;
    }

    /// <summary>
    /// Enable the reticle when the player picks up the GMD.
    /// </summary>
    private void EnableReticle()
    {
        if (reticle != null) reticle.enabled = true;
    }

    /// <summary>
    /// Disable the reticle when the player gets near the mineral box.
    /// </summary>
    private void DisableReticle()
    {
        if (reticle != null) reticle.enabled = false;
    }
}
