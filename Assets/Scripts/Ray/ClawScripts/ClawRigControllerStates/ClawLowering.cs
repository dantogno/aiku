using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawLowering : IClawRigControllerStates {

    /// <summary>
    /// A IclawRigController state solely for the perpose of our
    /// claw lowering down to pick up a box. 
    /// </summary>
    ClawRigController clawRigController;

    public ClawLowering(ClawRigController clawRigController)
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
