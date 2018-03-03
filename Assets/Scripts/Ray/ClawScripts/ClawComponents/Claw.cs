
using UnityEngine;

public class Claw : MonoBehaviour
{
    /// <summary>
    /// Our Claw, the claw is responsible for it's own lowering and raising animation
    /// as well as containing our box as a child object. 
    /// </summary>

    //The current target of our claw, used to determine if we parent a box to our claw
    public ClawLiftableCrate CurrentTargetedObject { get; set; }

    [Tooltip("This is how fast our crane lowers")]
     [SerializeField]
     float howFastDoesClawDrop = 5;

    // This is how long the pause between droping and lifting is.
    [Tooltip("How long is the pause between lowering and lifting back up")]
    [SerializeField]
    float howLongIsClawGrab;

    // This is the countDown for how long our pause 
    private float howLongIsClawGrabCountdown;

    //The starting height of our crane, used to tell how far to go back up. 
    private float craneOriginHeight;

    // Tells the claw how far to drop based on where
    // the target is.
    public float DesiredYCoordinate {  get; set; }

    // HACK, a refrence to our ClawRigController so we can tell if it is in a dropping or lifting state.
    private ClawRigController clawRigController;

    // This stops the lerp of the claw so it dosen't continue forever.
    private float terminationPoint = 0.1f; 

    
    private void Start()
    {
        craneOriginHeight = this.transform.position.y;
        clawRigController = this.gameObject.GetComponentInParent<ClawRigController>();
        setClawGrabTimer();
    }


    private void Update()
    {
        if (clawRigController.GetClawState() is ClawLifting)
        {
            clawLiftsUp();
        }

        if (clawRigController.GetClawState() is ClawLowering)
        {
            clawDropsDown();
        }
    }

    // Drops our claw down to pick up a box, transitions into a pause for the claw to close around the box. 
    private void clawDropsDown()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(transform.position.x, DesiredYCoordinate, transform.position.z), howFastDoesClawDrop * Time.deltaTime);
        if (this.transform.position.y - DesiredYCoordinate < terminationPoint)
        {
            closeClawOnLiftable();
        }
    }

    // A slight pause between our claw dropping and lifting to simulate the claw closing on the object. 
    private void closeClawOnLiftable()
    {
        if (howLongIsClawGrabCountdown > 0)
        {
            clawRigController.ClawAudioPlayer.PlayClawAttachAudio();
            howLongIsClawGrabCountdown -= Time.deltaTime;
        }
        else
        {
            // If the claw is holding a claw liftable object, drop it
            // Otherwise, pick up our targetedObject
            if (this.GetComponentInChildren<ClawLiftableCrate>() != null)
            {
                dropHeldObject();
            }
            else
            {
                grabTargetedObject();
            }
            setClawGrabTimer();
            clawRigController.SetClawState(clawRigController.GetClawLiftState());
        }
    }

    // Makes our claw lift back up after droping down to pick up a box. 
    private void clawLiftsUp()
    {
       this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(transform.position.x, craneOriginHeight, transform.position.z), howFastDoesClawDrop * Time.deltaTime);
        clawRigController.ClawAudioPlayer.PlayClawDropAudio();
        if (craneOriginHeight - transform.position.y  < terminationPoint)
         {
            if (CurrentTargetedObject != null)
            { 
                clawRigController.SetClawState(clawRigController.GetClawWithCrate());
            }
       
            else
                clawRigController.SetClawState(clawRigController.GetClawWithoutCrate());
        }

    }

    // Sets how long our simulated Grab will be
    private void setClawGrabTimer()
    {
        howLongIsClawGrabCountdown = howLongIsClawGrab;
    }

    // if the claw has a targeted object and it has finished dropping, we parent the object to the claw
    // and tell the object to move where the claw is. 
    private void grabTargetedObject()
    {
        if (CurrentTargetedObject != null)
        {
            CurrentTargetedObject.gameObject.transform.parent = this.transform;
            CurrentTargetedObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - DesiredYCoordinate, this.transform.position.z); // CurrentTargetedObject.TopOfCrate
            CurrentTargetedObject.LiftObject();
        }
    }

    // drops our lifted object, unparenting it and letting it fall to the ground. 
    private void dropHeldObject()
    {
        CurrentTargetedObject.DropObject();
        CurrentTargetedObject.gameObject.transform.parent = null;
        CurrentTargetedObject = null;
    }
}
