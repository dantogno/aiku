using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  A state for when our elevator is on the top floor and we press the button to make it go down
///   This will move the elevator down to the ElevatorOriginPoint until it hits an offset 
///  (Hack: reason for this is because lerps move infinitely closer to the end point, but never reach it.
///  I needed the elevator movment to end).
/// </summary>

public class ElevatorMovesDown : IElevatorStates
{

    // refrence to our elevator
    ElevatorScript rayElevator;

    // A "cut short" to ensure our elevator reaches a destination.
    private const float offset = 0.5f;

    public ElevatorMovesDown(ElevatorScript rayElevator)
    {
        this.rayElevator = rayElevator;
    }

    // Moves our elevator toward the ElevatorOriginPoint. If the player is on the panel, it will move them as well.
    // When our elevator reaches it's destination, it moves on to platformHasReachedDestination();
    public void moveToDestination(ElevatorPlatformTrigger elevatorPlatform)
    {

        elevatorPlatform.transform.position = Vector3.Lerp(elevatorPlatform.transform.position, rayElevator.ElevatorOriginPoint.position, rayElevator.ElevatorSpeed * Time.deltaTime);

        if (elevatorPlatform.IsPlayerOnElevator)
        {
            elevatorPlatform.PlayerOnPlatform.gameObject.transform.position = new Vector3(elevatorPlatform.PlayerOnPlatform.gameObject.transform.position.x, elevatorPlatform.gameObject.transform.position.y + elevatorPlatform.GetComponentInChildren<Collider>().bounds.size.y, elevatorPlatform.PlayerOnPlatform.gameObject.transform.position.z);
        }
    
        if (elevatorPlatform.transform.position.y <= rayElevator.ElevatorOriginPoint.position.y + offset)
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
