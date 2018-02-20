using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is applied to an object (assumedly a cryochamber power exchanger, but it could be anything - a shoe next to a Quidditch stadium, perhaps).
/// Just make sure that the object is a powerable or a child of a powerable (it will only work if the powerable has some power in it).
/// The script will simply pull up a slide describing the level for a few seconds.
/// </summary>

public class LevelTransitionPlaceholder : MonoBehaviour, IInteractable
{
    // This event broadcasts when a level is entered, and tells which level was entered.
    public delegate void LevelDelegate(Crewmember crewmemberLevelName);
    public static event LevelDelegate OnEnteredLevel;

    // This is used to distinguish which level is accessible through the object this script is applied to.
    public enum Crewmember { Norma, Ray, Trevor }

    [SerializeField, Tooltip("Whose level is this object associated with?")]
    private Crewmember crewmember;

    [SerializeField, Tooltip("This panel will pop up when a level is entered. Make sure it has a Text object as a child component!")]
    private GameObject levelTransitionPanel;

    [SerializeField, Tooltip("This is the text that will show up on the panel when the player enters a level.")]
    private string levelDescription;

    [SerializeField, Tooltip("This is how long the level description text will be on the screen for.")]
    private float waitTime = 3;

    // The only reason the cryochamber is included here is because the levels can only be entered if their associated cryochamber is at least partially powered.
    private PowerableObject connectedCryochamber;

    private bool levelsAreAccessible = false, hasEnteredLevel = false;

    #region Subscribe and unsubscribe to generator shutdown event (the player can only enter levels after the generator shutdown sequence).

    private void OnEnable()
    {
        EngineSequenceManager.OnShutdown += AllowPlayerToAccessLevels;
    }
    private void OnDisable()
    {
        EngineSequenceManager.OnShutdown -= AllowPlayerToAccessLevels;
    }

    #endregion

    private void Start()
    {
        InitializeReferences();

        levelTransitionPanel.SetActive(false);
    }

    public void Interact(GameObject agent)
    {
        bool canEnterLevel = levelsAreAccessible && connectedCryochamber.CurrentPower > 0;

        if (canEnterLevel)
        {
            StartCoroutine(EnterLevel());
        }
    }

    /// <summary>
    /// Initialize references to various components.
    /// </summary>
    private void InitializeReferences()
    {
        // The level transition object must be a child of a powerable.
        connectedCryochamber = GetComponentInParent<PowerableObject>();

        // The level transition panel must have a Text component as a child.
        Text levelText = levelTransitionPanel.GetComponentInChildren<Text>();

        levelText.text = levelDescription;
    }

    /// <summary>
    /// The player can only access levels after they finish the generator shutdown sequence.
    /// </summary>
    private void AllowPlayerToAccessLevels()
    {
        levelsAreAccessible = true;
    }

    /// <summary>
    /// When a level is entered, show a panel, broadcast an event, wait a few seconds, then hide the panel.
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnterLevel()
    {
        levelTransitionPanel.SetActive(true);

        if (OnEnteredLevel != null) OnEnteredLevel(crewmember);

        yield return new WaitForSeconds(waitTime);

        levelTransitionPanel.SetActive(false);
    }
}
