using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  An elevator for the Ray level 
///  The Elevator will be comprised of a platform,
///  A control switch, and a door. When the player stands on the platform and presses 
///  a button on the control switch, the elevator will move to a predestined location. 
/// </summary>
public class ElevatorScript : MonoBehaviour
{

    #region ElevatorComponents

    // Our elevator's platform that will be used to move the player up and down as well as detect if a player is standing on it.
    // This is script is assigned in Start() by a child object with a ElevatorPlatformTrigger.
    private ElevatorPlatformTrigger rayElevatorPlatform;

    #endregion

    #region Elevator Door Variables

    [SerializeField]
    private GameObject guardRails;

    [SerializeField]
    private GameObject guardRailsOriginPoint;

    [SerializeField]
    private GameObject guardRailsEndPoint;

    [SerializeField]
    private bool accessGranted;

    [SerializeField]
    private float countDownToDisappear;

    [SerializeField]
    private float timeToLowerRail;
    #endregion

    #region Elevator Variables

    [Tooltip("Transform of the lowest our elevator should go")]
    [SerializeField]
    private Transform elevatorOriginPoint;
    public Transform ElevatorOriginPoint { get { return elevatorOriginPoint; } }

    [Tooltip("Transform of the highest our elevator should go")]
    [SerializeField]
    private Transform elevatorEndPoint;
    public Transform ElevatorEndPoint { get { return elevatorEndPoint; } }

    [Tooltip("This number multiplied by time.DeltaTime. The speed at which our elevator platform travels")]
    [SerializeField]
    private float elevatorSpeed;
    public float ElevatorSpeed { get { return elevatorSpeed; } }


    [SerializeField] 
    private GameObject firstFloorElevatorCallTrigger;

    [SerializeField] 
    private GameObject secondFloorElevatorCallTrigger;
    #endregion

    #region ElevatorStates
    // The diffrent states our elevator can be in
    IElevatorStates elevatorStopped;
    IElevatorStates elevatorMovingUp;
    IElevatorStates elevatorMovingDown;

    // the curent state our elevator is in.
    IElevatorStates curentState;
    #endregion

    // initializing diffrent dependancies.
    void Start ()
    {
        rayElevatorPlatform = this.GetComponentInChildren<ElevatorPlatformTrigger>();

        elevatorStopped = new StopElevator();
        elevatorMovingUp = new ElevatorMoveUp(this);
        elevatorMovingDown = new ElevatorMovesDown(this);

        // Our elevator starts in the stopped state;
        curentState = elevatorStopped;
    }
    #region state Swap methods 
    // methods used to swap the elevator's state
    public void setElevatorState(IElevatorStates newElevatorState)
    {
        curentState = newElevatorState;
    }

    public IElevatorStates changeStateToStopped()
    {
        return elevatorStopped;
    }

    public IElevatorStates changeStateToFirstFloor()
    {
        return elevatorMovingUp;
    }

    public IElevatorStates changeStateToSecondFloor()
    {
        return elevatorMovingDown;
    }
    #endregion
    void Update ()
    {

        // based on our curent state, the elevatior moves to a target location, check the state scripts for more information.
        curentState.moveToDestination(rayElevatorPlatform);
    }

    // Checked to see which button is pressed, based on button pressed, our elevator changes state.
    public void Interact(GameObject agent)
    {
            if (agent.tag == "ElevatorButtonUp")
            {
                setElevatorState(changeStateToFirstFloor());
            }
            else if (agent.tag == "ElevatorButtonDown")
            {
                setElevatorState(changeStateToSecondFloor());
            }
        
    }
}
