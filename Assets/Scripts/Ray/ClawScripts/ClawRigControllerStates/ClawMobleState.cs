using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawMobleState : IClawRigControllerStates {


    /// <summary>
    /// Wrote this class so I would not repeat
    /// Movement code between ClawStateNotHoldingBox
    /// and ClawStateHoldingBox.
    /// </summary>
    
        // Refrence to our clawRigController
   protected ClawRigController clawRigController;

    // Claw moble state constructor, so it knows how to act on a claw rig controller. 
    public ClawMobleState(ClawRigController clawRigController)
    {
        this.clawRigController = clawRigController;
    }

    // When our claw is not moving, what happens? 
    public void ClawIsIdle()
    {
        clawRigController.ClawAudioPlayer.StopAudio();
    }

    // What happens when we want to drop the box or the claw
    // Either holding box or not holding box will decide what this will do.
    public virtual void DropClaw()
    {

    }

    // Moves the claw on the Z axis toward the camera.
    public void MoveClawDown()
    {
        if (clawRigController.hLowerBound.transform.InverseTransformPoint(clawRigController.Claw.transform.position).z < 0) 
        {
            clawRigController.Claw.transform.Translate(0, 0, (clawRigController.clawMoveSpeed * Time.deltaTime));
            clawRigController.HRail.transform.Translate(0, 0, (clawRigController.clawMoveSpeed * Time.deltaTime));
        }
        clawRigController.ClawAudioPlayer.PlayHorizontalMoveAudio();
    }

    // Moves the claw on the X axis toward the left of where the camera is facing.
    public void MoveClawLeft()
    {
        if (clawRigController.vUpperBound.transform.InverseTransformPoint(clawRigController.Claw.transform.position).x < 0)
        {
            clawRigController.Claw.transform.Translate(clawRigController.clawMoveSpeed * Time.deltaTime, 0, 0);
            clawRigController.VRail.transform.Translate((clawRigController.clawMoveSpeed * Time.deltaTime),0,0);
        }
        clawRigController.ClawAudioPlayer.PlayHorizontalMoveAudio();
    }

    // moves the claw on the X axis toward the right of where the camera is facing.
    public void MoveClawRight()
    {
        if (clawRigController.vLowerBound.transform.InverseTransformPoint(clawRigController.Claw.transform.position).x > 0)
        {
            clawRigController.Claw.transform.Translate(-(clawRigController.clawMoveSpeed) * Time.deltaTime, 0, 0);
            clawRigController.VRail.transform.Translate(-(clawRigController.clawMoveSpeed * Time.deltaTime), 0, 0);
        }
        clawRigController.ClawAudioPlayer.PlayHorizontalMoveAudio();
    }

    // Moves the claw along the Z axis away from the camera.
    public void MoveClawUp()
    {
        if (clawRigController.hUpperBound.transform.InverseTransformPoint(clawRigController.Claw.transform.position).z > 0) 
        {
            clawRigController.Claw.transform.Translate(0, 0, -(clawRigController.clawMoveSpeed * Time.deltaTime));
            clawRigController.HRail.transform.Translate(0, 0, -(clawRigController.clawMoveSpeed * Time.deltaTime));
        }
        clawRigController.ClawAudioPlayer.PlayHorizontalMoveAudio();
    }

}
   



