using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class toggles the text for Norma's lock combination in the hub, to remind the player what the combination is.
/// The script is applied to the object which has the Text component with the lock combination on it.
/// </summary>

public class LockComboText : MonoBehaviour
{
    [SerializeField, Tooltip("Norma's padlock")]
    private LockInteract padlock;

    private Text myText;

    private bool unlocked = false;

    private void OnEnable()
    {
        HubSceneChanger.FinishedLevel += TurnOnLockText;
        padlock.Unlocked += TurnOffLockText;
        
    }

    private void OnDisable()
    {
        HubSceneChanger.FinishedLevel -= TurnOnLockText;
        padlock.Unlocked -= TurnOffLockText;
    }

    /// <summary>
    /// When the player exits the Norma level, turn on the lock text.
    /// </summary>
    private void TurnOnLockText(HubSceneChanger.CrewmemberName crewmember)
    {
        myText = GetComponent<Text>();

        bool shouldEnableText =
            crewmember == HubSceneChanger.CrewmemberName.Norma && !unlocked && myText != null;

        if (shouldEnableText) myText.enabled = true;
    }

    /// <summary>
    /// When the player unlocks the lock, turn off the lock text.
    /// </summary>
    private void TurnOffLockText()
    {
        myText = GetComponent<Text>();

        if (myText != null) myText.enabled = false;

        unlocked = true;
    }
}
