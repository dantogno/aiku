using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClawRigControllerStates {
    void MoveClawUp();
    void MoveClawDown();
    void MoveClawLeft();
    void MoveClawRight();
    void DropClaw();
    void ClawIsIdle();
    
}
