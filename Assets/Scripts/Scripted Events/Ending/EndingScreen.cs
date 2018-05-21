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
    public static event Action AllocatedAllShipboardPowerToCryochambers, FoundAllPower;

    [SerializeField] private PowerableObject playerPowerable;

    // Every bit of power on the ship, excluding the player's.
    private static int allPowerOnTheShip = 22;

    private bool hasDisplayedFoundAllPowerMessage = false;
  
    private void OnEnable()
    {
        Cryochamber.AddedPowerToCryochamber += WaitToCheckScene;
    }
    private void OnDisable()
    {
        Cryochamber.AddedPowerToCryochamber -= WaitToCheckScene;
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
    public void CheckScene()
    {
        Cryochamber[] cryochambers = FindObjectsOfType<Cryochamber>();
                
        int totalPower = 0;

        // Add up all power in all cryochambers.
        foreach (Cryochamber c in cryochambers)
        {
            totalPower += c.CurrentPower;
        }

        // If the player has discovered all power on the ship, broadcast an event.
        if (totalPower + playerPowerable.CurrentPower >= allPowerOnTheShip && FoundAllPower != null && !hasDisplayedFoundAllPowerMessage)
        {
            FoundAllPower.Invoke();
            hasDisplayedFoundAllPowerMessage = true;
        }

        // If the player has collected all power on the ship, broadcast an event.
        if (totalPower >= allPowerOnTheShip && AllocatedAllShipboardPowerToCryochambers != null)
            AllocatedAllShipboardPowerToCryochambers.Invoke();
    }

    // TODO: Remove debug code
    //private void Update()
    //{
    //    if (Input.GetKey(KeyCode.Backspace))
    //    {
    //        if (AllocatedAllShipboardPowerToCryochambers != null)
    //            AllocatedAllShipboardPowerToCryochambers.Invoke();
    //    }
    //}
}
