using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class allows the player to enter levels after taking power from the stairwell power switch.
/// It is applied to the power switch at the bottom of the lower stairs.
/// </summary>

public class BottomStairsPowerSwitch : PowerSwitch, IInteractable
{
    public static event Action ExchangedPower;

    // This script only works once, the first time power is exchanged.
    private bool exchangedPowerForFirstTime = false;

    protected override void TransferPower(IPowerable otherPowerable)
    {
        base.TransferPower(otherPowerable);

        if (ExchangedPower != null && !exchangedPowerForFirstTime) ExchangedPower.Invoke();
        exchangedPowerForFirstTime = true;
    }
}
