using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Our elevator has stopped, it has no target location and sits still until
///  switched to a diffrent state.
/// </summary>

public class StopElevator : IElevatorStates
{
 
    public void moveToDestination(ElevatorPlatformTrigger elevatorPlatform)
    {
       // This is empty because we do not want our stopped elevator to move anywhere.
    }

    public void platformHasReachedDestination(ElevatorPlatformTrigger elevatorPlatform)
    {
       // This is empty because a stopped elevator has no destination. 
    }
}
