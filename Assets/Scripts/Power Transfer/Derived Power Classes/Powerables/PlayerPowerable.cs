using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for the player's own power reserve.
/// The script is applied to the player.
/// </summary>

public class PlayerPowerable : PowerableObject
{
    // These events are called near or at the very end of the game, when the player sacrifices themself for the crew.  :(
    public static event Action TransferredPartOfPowerReserve, TransferredEntirePowerReserve;

    // This bool is set to true when the player is using their own power reserve instead of collected power.
    private bool transferringOwnPower = false;

    private void OnEnable()
    {
        EndingScreen.AllocatedAllShipboardPowerToCryochambers += SetTransferringOwnPower;
    }
    private void OnDisable()
    {
        EndingScreen.AllocatedAllShipboardPowerToCryochambers -= SetTransferringOwnPower;
    }

    /// <summary>
    /// When all power on the ship is spent, we use our own to save the crew.
    /// </summary>
    private void SetTransferringOwnPower()
    {
        transferringOwnPower = true;

        // Now that we are using our own power, we have a full power supply to work with.
        PowerOn();
    }

    /// <summary>
    /// Subtracts powerToSubtract from CurrentPower if the result is zero or more.
    /// Powers off if the result is zero.
    /// Also broadcasts event for other scripts to listen for.
    /// </summary>
    /// <param name="powerToSubtract"></param>
    public override void SubtractPower(int powerToSubtract)
    {
        base.SubtractPower(powerToSubtract);

        if (transferringOwnPower && TransferredPartOfPowerReserve != null)
            TransferredPartOfPowerReserve.Invoke();
    }

    /// <summary>
    /// Sets CurrentPower to zero.
    /// Sets IsFullyPowered to false.
    /// Invokes OnPoweredOff event.
    /// Also broadcasts event for other scripts to listen for.
    /// </summary>
    public override void PowerOff()
    {
        base.PowerOff();

        if (transferringOwnPower && TransferredEntirePowerReserve != null)
            TransferredEntirePowerReserve.Invoke();
    }
}
