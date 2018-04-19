using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This is used to detect if there is a player standing on the elevator
///  if they are, they will move with the platform, 
///  if not the platform will move on it's own.
/// </summary>

[RequireComponent(typeof(Collider))]
public class ElevatorPlatformTrigger :MonoBehaviour
{
    private bool isPlayerOnElevator;
    public bool IsPlayerOnElevator { get { return isPlayerOnElevator; } }

    private GameObject playerOnPlatform;
    public GameObject PlayerOnPlatform { get { return playerOnPlatform; } set { playerOnPlatform = value; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerOnElevator = true;

            // Gets a refrence of our player so we can move them with the elevator. 
            playerOnPlatform = other.gameObject;

            //setElevatorState(elevatorIsOnTheFirstFloor);
            //MoveElevatorUP();
            //timeToLowerRail = 5;
            //accessGranted = true;
            //countDownToDisappear = 3;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerOnElevator = false;
            //player = other.gameObject;

            //setElevatorState(elevatorIsOnTheFirstFloor);
            //MoveElevatorUP();
            //timeToLowerRail = 5;
            //accessGranted = true;
            //countDownToDisappear = 3;
        }
    }
}
