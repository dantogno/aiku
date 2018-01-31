using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Elevators in this game are automatic. When powered, the elevator will activate when the player stands on its platform.
/// Elevators are one-way. The player can only use them to access floors above them, never below.
/// The logic and data within this class are considered to be self-explanatory, hence the lack of documentation.
/// </summary>

public class Elevator : PowerableObject
{
    private Animator myAnimator;

    protected override void Start()
    {
        base.Start();

        myAnimator = GetComponent<Animator>();
    }

    /// <summary>
    /// Sets CurrentPower to zero.
    /// Sets IsFullyPowered to false.
    /// Invokes OnPoweredOff event.
    /// Lowers the elevator.
    /// </summary>
    public override void PowerOff()
    {
        base.PowerOff();

        if (!IsFullyPowered) Descend();
    }

    private void Descend()
    {
        myAnimator.SetTrigger("Descend");
    }
}
