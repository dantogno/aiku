using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The cryochambers are the most important powerable object on the ship!
/// When a cryochamber is powered, the player can enter its crewmember's level.
/// The script is applied to the cryochamber parent object.
/// </summary>

public class Cryochamber : PowerableObject
{
    public static event Action AddedPowerToCryochamber;

    [SerializeField, Tooltip("Light to toggle based on power level.")]
    private GameObject poweredLight, emergencyLight;

    [SerializeField, Tooltip("When the cryochamber is fully powered, activate the scene changer and deactivate the ordinary monitor.")]
    private GameObject sceneChanger, ordinaryMonitor;

    // The player can only enter levels after the generator explodes and before they have transferred all available power to the cryochambers.
    private bool canEnterLevels = true;

    private void OnEnable()
    {
        EngineSequenceManager.OnShutdown += AllowPlayerToEnterLevelsAndShutDown;
        EndingScreen.AllocatedAllShipboardPowerToCryochambers += PreventPlayerFromEnteringLevels;
    }
    private void OnDisable()
    {
        EngineSequenceManager.OnShutdown -= AllowPlayerToEnterLevelsAndShutDown;
    }

    /// <summary>
    /// When the generator explodes, the player can enter levels when there is power in the cryochamber.
    /// </summary>
    private void AllowPlayerToEnterLevelsAndShutDown()
    {
        canEnterLevels = true;

        PowerOff();
    }

    /// <summary>
    /// After the player has collected all the ship's power, entering levels is no longer allowed.
    /// </summary>
    private void PreventPlayerFromEnteringLevels()
    {
        canEnterLevels = false;

        sceneChanger.SetActive(false);
        ordinaryMonitor.SetActive(true);
    }

    /// <summary>
    /// Adds powerToAdd to CurrentPower if the result is less than RequiredPower.
    /// Powers on if the result is equal to RequiredPower.
    /// Also turns on the cryochamber's scene changer.
    /// </summary>
    /// <param name="powerToAdd"></param>
    public override void AddPower(int powerToAdd)
    {
        base.AddPower(powerToAdd);

        if (canEnterLevels)
        {
            sceneChanger.SetActive(true);
            ordinaryMonitor.SetActive(false);

            if (AddedPowerToCryochamber != null) AddedPowerToCryochamber.Invoke();
        }
    }

    /// <summary>
    /// Subtracts powerToSubtract from CurrentPower if the result is zero or more.
    /// Powers off if the result is zero.
    /// Also changes color of cryochamber light.
    /// </summary>
    /// <param name="powerToSubtract"></param>
    public override void SubtractPower(int powerToSubtract)
    {
        base.SubtractPower(powerToSubtract);

        poweredLight.SetActive(false);
        emergencyLight.SetActive(true);
    }

    /// <summary>
    /// Sets CurrentPower to RequiredPower.
    /// Sets IsFullyPowered to true.
    /// Invokes OnPoweredOn event.
    /// Also changes color of cryochamber light.
    /// </summary>
    public override void PowerOn()
    {
        base.PowerOn();

        poweredLight.SetActive(true);
        emergencyLight.SetActive(false);
    }

    /// <summary>
    /// Sets CurrentPower to zero.
    /// Sets IsFullyPowered to false.
    /// Invokes OnPoweredOff event.
    /// Also deactivates the cryochamber's scene changer and changes the color of its light.
    /// </summary>
    public override void PowerOff()
    {
        base.PowerOff();

        sceneChanger.SetActive(false);
        ordinaryMonitor.SetActive(true);
        poweredLight.SetActive(false);
        emergencyLight.SetActive(true);
    }
}
