using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawStateHoldingBox : ClawMobleState {

    /// <summary>
    /// A claw state for when we are holding a box
    /// This should override what happens when we press interact. 
    /// setting the box down rather than picking one up. 
    /// </summary>

     // A constructor to tell Claw MobleState what a clawRigController is.
    public ClawStateHoldingBox(ClawRigController clawRigController) : base(clawRigController)
    {

    }

    // When we press interact we 
    //drop the claw to a desired Coordinate
    //Tell the audio player to play the appropriate sound cue
    // Set our claw state to GetClawLowerState(), which triggers our claw lowering animation within claw.
    public override void DropClaw()
    {
        clawRigController.Claw.DesiredYCoordinate = clawRigController.Claw.transform.position.y;
        clawRigController.ClawAudioPlayer.PlayClawDropAudio();
        clawRigController.SetClawState(clawRigController.GetClawLowerState());
    }
}
