using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class toggles the animation on the laundry machine when its power is exchanged.
/// The script is applied to the laundry machine, and must have an Animator component.
/// </summary>

public class LaundryMachine : PowerableObject
{
    /// <summary>
    /// Sets CurrentPower to RequiredPower.
    /// Sets IsFullyPowered to true.
    /// Invokes OnPoweredOn event.
    /// Also turns on the animation.
    /// </summary>
    public override void PowerOn()
    {
        base.PowerOn();

        GetComponent<Animator>().speed = 1;
    }

    /// <summary>
    /// Sets CurrentPower to zero.
    /// Sets IsFullyPowered to false.
    /// Invokes OnPoweredOff event.
    /// Also turns off the animation.
    /// </summary>
    public override void PowerOff()
    {
        base.PowerOff();

        GetComponent<Animator>().speed = 0;
    }
}
