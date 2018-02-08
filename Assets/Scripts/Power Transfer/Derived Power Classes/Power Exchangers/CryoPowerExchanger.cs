using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The power exchangers for cryochambers are specialized.
/// Unlike power switches, cryochamber power exchangers operate incrementally, rather than all-or-nothing.
/// </summary>

public class CryoPowerExchanger : PowerExchanger
{
    /// <summary>
    /// Transfer power between an interacting agent (otherPowerable) and the power exchanger's connected powerable.
    /// </summary>
    /// <param name="otherPowerable"></param>
    protected override void TransferPower(IPowerable otherPowerable)
    {
        bool otherPowerableHasPower = otherPowerable.CurrentPower > 0,
            cryochamberHasRoomForMorePower = connectedPowerable.CurrentPower < connectedPowerable.RequiredPower;

        // If the other powerable can deposit power into the connected powerable...
        if (otherPowerableHasPower && cryochamberHasRoomForMorePower)
        {
            ReceivePowerFromOtherPowerable(otherPowerable);
        }

        // Otherwise, the other powerable can withdraw power from the connected powerable.
        else
        {
            AllowOtherPowerableToExtractPowerFromConnectedPowerable(otherPowerable);
        }
    }

    /// <summary>
    /// Check how much power the connected powerable can accept from otherPowerable.
    /// Add that amount of power to the connected powerable and subtract it from otherPowerable.
    /// </summary>
    /// <param name="otherPowerable"></param>
    private void ReceivePowerFromOtherPowerable(IPowerable otherPowerable)
    {
        bool canAcceptPower =
            connectedPowerable.CurrentPower + otherPowerable.CurrentPower <= connectedPowerable.RequiredPower;

        if (canAcceptPower)
        {
            // Add otherPowerable.CurrentPower to connected powerable first, because otherPowerable.CurrentPower will change.
            connectedPowerable.AddPower(otherPowerable.CurrentPower);

            // Deplete otherPowerable.CurrentPower after adding it to the connected powerable.
            otherPowerable.SubtractPower(otherPowerable.CurrentPower);
        }
        else
        {
            // Deplete otherPowerable.CurrentPower first because connected powerable's power differential will change.
            otherPowerable.SubtractPower(connectedPowerable.RequiredPower - connectedPowerable.CurrentPower);

            // Add connected powerable's power differential to connected powerable after subtracting it from otherPowerable.CurrentPower.
            connectedPowerable.AddPower(connectedPowerable.RequiredPower - connectedPowerable.CurrentPower);
        }
    }

    /// <summary>
    /// Check how much power otherPowerable can withdraw from the connected powerable.
    /// Subtract that amount of power from the connected powerable and add it to otherPowerable.
    /// </summary>
    /// <param name="otherPowerable"></param>
    private void AllowOtherPowerableToExtractPowerFromConnectedPowerable(IPowerable otherPowerable)
    {
        bool hasMorePowerThanOtherPowerableCanHold =
            connectedPowerable.CurrentPower > otherPowerable.RequiredPower - otherPowerable.CurrentPower;

        if (hasMorePowerThanOtherPowerableCanHold)
        {
            // Deplete connectedPowerable.CurrentPower first because other powerable's power differential will change.
            connectedPowerable.SubtractPower(otherPowerable.RequiredPower - otherPowerable.CurrentPower);

            // Add other powerable's power differential to other powerable after subtracting it from connectedPowerable.CurrentPower.
            otherPowerable.AddPower(otherPowerable.RequiredPower - otherPowerable.CurrentPower);
        }
        else
        {
            // Add connectedPowerable.CurrentPower to other powerable first, because connectedPowerable.CurrentPower will change.
            otherPowerable.AddPower(connectedPowerable.CurrentPower);

            // Deplete connectedPowerable.CurrentPower after adding it to the other powerable.
            connectedPowerable.SubtractPower(connectedPowerable.CurrentPower);
        }
    }
}
