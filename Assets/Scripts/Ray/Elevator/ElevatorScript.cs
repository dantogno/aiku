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

    //[SerializeField]
    //private float timeToGoDown;


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
        //guardRails.transform.position = guardRailsOriginPoint.transform.position;
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
    void Update () {

        // based on our curent state, the elevatior moves to a target location, check the state scripts for more information.
        curentState.moveToDestination(rayElevatorPlatform);
        //if (!secondFloorElevatorCallTrigger.activeSelf && isOnFirstFloor == true)
        //{
        //    if (isOnFirstFloor)
        //    {
        //        MoveElevatorUP();
        //    }       
        //}
        //if (!firstFloorElevatorCallTrigger.activeSelf)
        //{         
        //    if (isOnSecondFloor)
        //    {
        //        MoveElevatorDown();
        //    }
        //}
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        player = other.gameObject;
            
    //        //setElevatorState(elevatorIsOnTheFirstFloor);
    //        //MoveElevatorUP();
    //        //timeToLowerRail = 5;
    //        //accessGranted = true;
    //        //countDownToDisappear = 3;
    //    }     
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        Debug.Log("We have exited");
    //        //other.gameObject.transform.parent = null;
            
    //        //setElevatorState(elevatorIsOnTheFirstFloor);
    //        //MoveElevatorUP();
    //        //timeToLowerRail = 5;
    //        //accessGranted = true;
    //        //countDownToDisappear = 3;
    //    }
    //}

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

    //public void AutoElevatorGoDown()
    //{   
    //    if (timeToGoDown <= 0)
    //    {
    //        //accessGranted = false;
    //       // LowerGuardRails();
    //    }
    //}

    //public void MoveElevatorDown()
    //{
    //    //RaiseGuardRails();
    //   // isOnSecondFloor = false;
    //    //Vector3 startingPosition = new Vector3(rayElevatorPlatform.transform.position.x, rayElevatorPlatform.transform.position.y, rayElevatorPlatform.transform.position.z);
    //    //rayElevatorPlatform.transform.position = Vector3.Lerp(startingPosition, elevatorOriginPoint.position, Time.deltaTime);
    //    //timeToLowerRail -= Time.deltaTime;    
    //}

    //public void MoveElevatorUP()
    //{
    //    // RaiseGuardRails();
    //    // isOnFirstFloor = false;    
    //    Vector3 startingPosition = new Vector3(rayElevatorPlatform.transform.position.x, rayElevatorPlatform.transform.position.y, rayElevatorPlatform.transform.position.z);
    //    rayElevatorPlatform.transform.position = Vector3.Lerp(startingPosition, elevatorEndPoint.position, Time.deltaTime);
    //    timeToGoDown = 7;
    //}

    //private void AutoLowerRail()
    //{          
    //    //timeToLowerRail-= Time.deltaTime;

    //    //if (timeToLowerRail <= 0)
    //    //{
    //    //    accessGranted = false;
    //    //    //LowerGuardRails();
    //    //}
    //}

    //private void RaiseGuardRails()
    //{  
    //    guardRails.SetActive(true);
    //    Vector3 startingPosition = new Vector3(guardRails.transform.position.x, guardRails.transform.position.y, guardRails.transform.position.z);
    //    guardRails.transform.position = Vector3.Lerp(startingPosition, guardRailsEndPoint.transform.position, Time.deltaTime);

    //}
    //private void LowerGuardRails()
    //{
    //    countDownToDisappear -= Time.deltaTime;
    //    Vector3 startingPosition = new Vector3(guardRails.transform.position.x, guardRails.transform.position.y, guardRails.transform.position.z);
    //    guardRails.transform.position = Vector3.Lerp(startingPosition, elevatorOriginPoint.transform.position, Time.deltaTime);
    //    if (countDownToDisappear <= 0)
    //    {
    //        guardRails.SetActive(false);
    //    }
    //}

}
