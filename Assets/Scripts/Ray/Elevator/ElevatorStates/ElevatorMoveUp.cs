using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  The state our elevator will be in when the up arrow is clicked on the control panel. 
///  This will move the elevator up to the ElevatorEndPoint until it hits an offset 
///  (Hack: reason for this is because lerps move infinitly closer to the end point, but never reach it.
///  I needed the elevator movment to end). 
/// </summary>

public class ElevatorMoveUp : IElevatorStates
{
    // refrence to our elevator. 
    private ElevatorScript rayElevator;

    // A "cut short" to ensure our elevator reaches a destination.
    private const float offset = 0.5f;


    public ElevatorMoveUp(ElevatorScript rayElevator)
    {
        this.rayElevator = rayElevator;
    }

    // Moves our elevator toward the ElevatorEndPoint. If the player is on the panel, it will move them as well.
    // When our elevator reaches it's destination, it moves on to platformHasReachedDestination();
    public void moveToDestination(ElevatorPlatformTrigger elevatorPlatform)
     {
        elevatorPlatform.gameObject.transform.position = Vector3.Lerp(elevatorPlatform.gameObject.transform.position, rayElevator.ElevatorEndPoint.position, rayElevator.ElevatorSpeed * Time.deltaTime);

        if (elevatorPlatform.IsPlayerOnElevator)
        {
            elevatorPlatform.PlayerOnPlatform.gameObject.transform.position = new Vector3(elevatorPlatform.PlayerOnPlatform.gameObject.transform.position.x, elevatorPlatform.gameObject.transform.position.y + elevatorPlatform.GetComponentInChildren<Collider>().bounds.size.y, elevatorPlatform.PlayerOnPlatform.gameObject.transform.position.z);
        }

        if(elevatorPlatform.transform.position.y >= (rayElevator.ElevatorEndPoint.position.y - offset)) 
        {
            platformHasReachedDestination(elevatorPlatform);
        }
    }

    // changes elevator state to stopped 
    public void platformHasReachedDestination(ElevatorPlatformTrigger elevatorPlatform)
    {
        rayElevator.setElevatorState(rayElevator.changeStateToStopped());
    }
}
