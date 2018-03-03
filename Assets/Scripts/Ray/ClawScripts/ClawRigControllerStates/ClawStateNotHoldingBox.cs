using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawStateNotHoldingBox : ClawMobleState
{
    /// <summary>
    /// How our Claw acts when it is not holding a box
    /// When claw is interacted with, the claw will drop down and pick up a box if one is targeted
    /// if not targeted, the claw will simply drop to the ground and then retract to it's original
    /// position.
    /// </summary>

    public ClawStateNotHoldingBox(ClawRigController clawRigController): base(clawRigController)
    {
    }

    public override void DropClaw()
    {
        if (clawRigController.ClawRayCaster.CurentCraneTarget != null)
        {
            clawRigController.Claw.CurrentTargetedObject = clawRigController.ClawRayCaster.CurentCraneTarget;
            clawRigController.Claw.DesiredYCoordinate = (clawRigController.Claw.transform.position.y - clawRigController.ClawRigOffset) - clawRigController.ClawRayCaster.getRaycastHit().distance;
        }
        else
        {
            clawRigController.Claw.DesiredYCoordinate = clawRigController.Claw.transform.position.y - clawRigController.ClawRayCaster.getRaycastHit().distance;
        }
        clawRigController.ClawAudioPlayer.PlayClawDropAudio();
        clawRigController.SetClawState(clawRigController.GetClawLowerState());
    }
}
