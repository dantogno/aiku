using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class RetractableStairs : PowerableObject
{
    private Animator myAnimator;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    public override void PowerOn()
    {
        base.PowerOn();

        ExtendStairs();
    }

    public override void PowerOff()
    {
        base.PowerOff();

        RetractStairs();
    }

    private void ExtendStairs()
    {
        myAnimator.SetTrigger("Extend");
    }

    private void RetractStairs()
    {
        myAnimator.SetTrigger("Retract");
    }
}
