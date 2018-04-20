using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  An interface for our elevator states
///  An elevator state can either move to its destination
///  or alert the elevator that the platform has reached the destination
///  There are 3 elevator states:
///  moving up,
///  moving down ,
///  and stopped.
/// </summary>
/// 
public interface IElevatorStates
{

    void moveToDestination(ElevatorPlatformTrigger elevatorPlatform);
    void platformHasReachedDestination(ElevatorPlatformTrigger elevatorPlatform);
}
