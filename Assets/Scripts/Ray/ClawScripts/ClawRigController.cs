using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawRigController : MonoBehaviour
{
    #region Editor fields
    [Tooltip("The Transform of the HorizontalMovingRail child of this Game Object")]
    [SerializeField] private Transform hRail;
    public Transform HRail { get { return hRail; } }
    [Tooltip("The Transform of the VerticalMovingRail child of this Game Object")]
    [SerializeField] private Transform vRail; //The vertical rail for the claw
    public Transform VRail { get { return vRail; } }   
    
    [Tooltip("The distance from its destination the claw will stop")]
    //This is the distance the claw will stop from either 
    //the object it is picking up or the rails when it comes up
    [SerializeField]
    float clawRigOffset = 1.9f;
    public float ClawRigOffset { get { return clawRigOffset; } }

    // Our claw's audio component 
    public ClawAudioPlayer ClawAudioPlayer { get; private set; }

    // Our claw's Ray casting component
    public ClawRayCaster ClawRayCaster { get; private set; }

    // The physical claw in the game, responcible for lowering and lifting back up
    public Claw Claw{ get; private set; }

    /// <summary>
    /// All four of the below variables are to limit the claws movement on the horizontal plane
    /// the difference between horizontal and vertical is to distinguish between up - down (w,s)
    /// and left - right (a,d).  Not actual vertical and horizontal movement so their names may
    /// be confusing or missleading but I hope this helps to clarify
    /// </summary>
    [Tooltip("The Transform of the hLowerBound child of this Game Object")]
    public Transform hLowerBound;
    [Tooltip("The Transform of the hUpperBound child of this Game Object")]
    public Transform hUpperBound;
    [Tooltip("The Transform of the vLowerBound child of this Game Object")]
    public Transform vLowerBound;
    [Tooltip("The Transform of the vUpperBound child of this Game Object")]
    public Transform vUpperBound;

    [Tooltip("The speed the claw moves via (w, a, s, d or up, down, left, right)")]
    public float clawMoveSpeed = 3.0f;
    #endregion


    #region Private fields
    // These are the states for our crane.
    IClawRigControllerStates rigIsOff;
    IClawRigControllerStates rigIsEmpty;
    IClawRigControllerStates rigIsHoldingBox;
    IClawRigControllerStates clawLowering;
    IClawRigControllerStates clawLifting;
    // The current state our claw rig is in.
    IClawRigControllerStates currentClawRigState;

    // here is our control map; 
    private ClawRigControlMap input;
    #endregion

    #region FunctionsToSwitchStates
    // Returns our various claw states so it can switch states.
    public void SetClawState(IClawRigControllerStates newClawRigState)
    {
        currentClawRigState = newClawRigState;
    }

    public IClawRigControllerStates GetClawState()
    {
        return currentClawRigState;
    }

    public IClawRigControllerStates GetPowerOff()
    {
        ClawAudioPlayer.StopAudio();
        return rigIsOff;
    }

    public IClawRigControllerStates GetClawWithoutCrate()
    {
        return rigIsEmpty;
    }

    public IClawRigControllerStates GetClawWithCrate()
    {
        return rigIsHoldingBox;
    }

    public IClawRigControllerStates GetClawLowerState()
    {
        return clawLowering;
    }

    public IClawRigControllerStates GetClawLiftState()
    { 
        return clawLifting;
    }
    #endregion


    public void TurnRigOn()
    {
        SetClawState(GetClawWithoutCrate());
    }

    public void TurnRigOff()
    {
        SetClawState(GetPowerOff());
    }

    void Start ()
    {
        // Sets our AudioPlayer to the RigController
        ClawAudioPlayer = this.GetComponentInChildren<ClawAudioPlayer>();

        // Sets our ClawRayCaster to the RigController
        ClawRayCaster = this.GetComponentInChildren<ClawRayCaster>();

        // Sets our Claw to the RigController
        Claw = this.GetComponentInChildren<Claw>();

        input = new ClawRigControlMap();

        //Moving the horizontal railing so that it is directly over the claw
        hRail.position = new Vector3(hRail.position.x, hRail.position.y, Claw.transform.position.z);

        //Moving the vertical railing so that it is directly over the claw
        vRail.position = new Vector3(Claw.transform.position.x, vRail.position.y, vRail.position.z);

        // initializing our claw rig states with a refrence to this.
        rigIsOff = new ClawStatePowerOff(this);
        rigIsEmpty = new ClawStateNotHoldingBox(this);
        rigIsHoldingBox = new ClawStateHoldingBox(this);
        clawLowering = new ClawLowering(this);
        clawLifting = new ClawLifting(this);

        // setting the initial state to off. 
        currentClawRigState = rigIsOff;
    }
	
	void Update ()
    {
        interpretRigMap();
    }

    // Used to tell if the user entered input and if so, which key
    // this calls the function for the crane based on what state the crane is in.
    private void interpretRigMap()
    {
        bool isInputUsed = false; 
        foreach (var item in input.keysPressed)
        {
            if (Input.GetKey(item.Key))
            {
                isInputUsed = true;
                switch (item.Value)
                {
                    case "Move Up":
                        {
                            currentClawRigState.MoveClawUp();
                            break;
                        }

                    case "Move Down":
                        {

                            currentClawRigState.MoveClawDown();
                            break;
                        }

                    case "Move Left":
                        {

                            currentClawRigState.MoveClawLeft();
                            break;
                        }

                    case "Move Right":
                        {

                            currentClawRigState.MoveClawRight();
                            break;
                        }

                    case "Drop Claw":
                        {
                            currentClawRigState.DropClaw();
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }
        if (!isInputUsed)
        {
            currentClawRigState.ClawIsIdle();
        }
    }
}
