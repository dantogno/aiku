using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// This class controls how the player interacts with the cryochambers after all levels have been played through.
/// The script is applied to the monitor GameObjects which are children of the level transition objects.
/// </summary>

public class EndingScreen : MonoBehaviour
{
    // Three Actions, one for when the functionality of the scene changer should change, one for the end of the game,
    // and one for when the player transfers their power - the player's final act in the game.
    public static event Action AllocatedAllShipboardPowerToCryochambers;

    // Every bit of power on the ship, excluding the player's.
    private static int allPowerOnTheShip = 22;

    // The number of crewmembers the player has transferred power to.
    private static int crewmembersSaved;

    [SerializeField, Tooltip("The name of the crewmember, to appear in a prompt in the text appearing next to the monitor.")]
    private string crewmemberName;

    [SerializeField, Tooltip("Amount of time to wait after transferring power to check if all power has been collected.")]

    // Scene changer attached to this GameObject.
    private HubSceneChanger mySceneChanger;

    private void Awake()
    {
        InitializeReferences();
    }

    private void OnEnable()
    {
        Cryochamber.AddedPowerToCryochamber += WaitToCheckScene;
    }
    private void OnDisable()
    {
        Cryochamber.AddedPowerToCryochamber -= WaitToCheckScene;
    }

    private void InitializeReferences()
    {
        mySceneChanger = GetComponent<HubSceneChanger>();
    }

    /// <summary>
    /// Wait to check power for a little bit, because the other subscribing events get mixed-up while power is being transferred.
    /// </summary>
    private void WaitToCheckScene()
    {
        Invoke("CheckScene", 1);
    }

    /// <summary>
    /// Check if the player has collected all power on the ship.
    /// </summary>
    private void CheckScene()
    {
        Cryochamber[] cryochambers = FindObjectsOfType<Cryochamber>();
                
        int totalPower = 0;

        // Add up all power in all cryochambers.
        foreach (Cryochamber c in cryochambers)
        {
            totalPower += c.CurrentPower;
        }

        // If the player has collected all power on the ship, broadcast an event.
        if (totalPower >= allPowerOnTheShip && AllocatedAllShipboardPowerToCryochambers != null)
            AllocatedAllShipboardPowerToCryochambers.Invoke();
    }
}
