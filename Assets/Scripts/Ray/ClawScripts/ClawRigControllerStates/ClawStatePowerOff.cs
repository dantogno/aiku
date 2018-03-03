using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawStatePowerOff : IClawRigControllerStates{

    /// <summary>
    ///  A IClawRigControllerState 
    /// </summary>

    ClawRigController clawRigController;

    public ClawStatePowerOff(ClawRigController clawRigController)
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
