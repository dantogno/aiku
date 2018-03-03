using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawLifting : IClawRigControllerStates {

    /// <summary>
    /// A IClawRigControllerState solely for the purpose of telling the claw
    /// to play it's lift animation. 
    /// </summary>

      ClawRigController clawRigController;

     public ClawLifting(ClawRigController clawRigController)
    {
        this.clawRigController = clawRigController;
    }

    public void ClawIsIdle()
    {
        
    }

    public void DropClaw()
    {
        
    }

    public void MoveClawDown()
    {
        
    }

    public void MoveClawLeft()
    {
        
    }

    public void MoveClawRight()
    {
        
    }

    public void MoveClawUp()
    {
        
    }
}
