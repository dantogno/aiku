using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages when the instruction text for the GMD is enabled/disabled
/// </summary>
public class ManageGMDInstructions : MonoBehaviour
{
    private Text instructionText;

    /// <summary>
    /// Gets the text component of the UI object
    /// </summary>
    private void Start()
    {
        instructionText = GetComponent<Text>();
    }

    private void OnEnable()
    {
        PickupGMD.PickedUpGMD += ActivateInstructions;
        PortalNew.PlayerTeleported += DeactivateInstructions;
    }

    private void OnDisable()
    {
        PickupGMD.PickedUpGMD -= ActivateInstructions;
        PortalNew.PlayerTeleported -= DeactivateInstructions;
    }

    /// <summary>
    /// Enables the instruction text
    /// </summary>
    private void ActivateInstructions()
    {
        instructionText.enabled = true;
    }

    /// <summary>
    /// Disables the instruction text and this script
    /// </summary>
    private void DeactivateInstructions()
    {
        instructionText.enabled = false;
        this.enabled = false;
    }
}
