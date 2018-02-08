using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script can go on any object that can be picked up by a player or an agent.
/// It implements IInteractable, and can thus be interacted with.
/// The object is placed at a position relative to the player or the player camera by
/// an offset in the X Y and Z axes.
/// The object can be dropped by pressing the "Interact" button.
/// As of right now, this functions such that if a player is holding an object and attempts
/// to interact with something else, they drops the held object first.
/// </summary>
public class PickUp : MonoBehaviour, IInteractable
{
    /*Special note:
     When testing these values out, an x limit of -1.1 to 1.1 and a y limit of 
     -0.6 to 0.6 worked for me as the edges of the screen. I don't know if this scales
     with increased screen size.*/
    [Tooltip("Sets the X offset relative to the agent when the object is picked up.")]
    [SerializeField]
    [Range(-1.1f, 1.1f)]
    private float xOffset = 1.1f;

    [Tooltip("Sets the Y offset relative to the agent when the object is picked up.")]
    [SerializeField]
    [Range(-0.6f, 0.6f)]
    private float yOffset = -0.6f;

    [Tooltip("Sets the Z offset relative to the agent when the object is picked up.")]
    [SerializeField]
    [Range(1f, 3f)]
    private float zOffset = 1f;

    [Tooltip("The lerp speed of the object when it is picked up.")]
    [SerializeField]
    [Range(0f, 1f)]
    private float lerpSpeed = 0.1f;

    [Tooltip("Sets the object as a child of the Player instead of the Camera.")]
    [SerializeField]
    private bool childToPlayer = false;

    //The offset position is determined by xOffset, yOffset and zOffset
    //It is relative to the player.
    private Vector3 offsetPosition;
    //The rigidbody currently attached to the GameObject (if any).
    private Rigidbody thisRigidbody;
    //When a player drops an object, we want to make sure they don't interact
    //with it again on the same frame.
    private bool wasDroppedThisFrame = false;

    //Is the object currently being held?
    protected bool isHeld;
    //A reference to the agent holding the object (if any).
    protected GameObject holder;

    protected virtual void Start()
    {
        offsetPosition = new Vector3(xOffset, yOffset, zOffset);
        thisRigidbody = this.gameObject.GetComponent<Rigidbody>();
        isHeld = false;
        holder = null;
    }

    protected virtual void Update()
    { 
        //Reset the variable so the the object can be interacted with again.
        wasDroppedThisFrame = false;
        if(isHeld && Input.GetButtonDown("Interact"))
        {
            //Drop the object and make a note that it was dropped this frame.
            DropThis();
            wasDroppedThisFrame = true;
        }
    }

    protected virtual void FixedUpdate()
    {
        //Move the object to a position relative to the holder if it is currently
        //being held.
        // Also makes sure that the object stays in its place in case of wall
        //collision or other physics events.
        if(isHeld && holder != null)
        {
            LerpObjectToDesiredPosition();
        }
    }

    /// <summary>
    /// Pick the object up when it is interacted with.
    /// </summary>
    /// <param name="agentInteracting">The agent interacting with the object</param>
    public virtual void Interact(GameObject agentInteracting)
    {
        //Only pick the object up if it is not currently being held and if it was not dropped this frame.
        if (!isHeld && !wasDroppedThisFrame)
        {
            PickThisUp(agentInteracting);
        }
    }

    /// <summary>
    /// Lerps the object to a specified position. The lerp speed can be set in the editor.
    /// </summary>
    private void LerpObjectToDesiredPosition()
    {
        //Translate the offset position into world space to get the world coordinate.
        Vector3 worldCoordinate = Vector3.Lerp(this.transform.position, holder.transform.TransformPoint(offsetPosition), lerpSpeed);
        if (thisRigidbody != null)
        {
            //Move the object with the physics engine if it has a rigidbody.
            thisRigidbody.MovePosition(worldCoordinate);
        }
        else
        {
            //If not, just set its position.
            this.transform.position = worldCoordinate;
        }
    }

    /// <summary>
    /// The object gets picked up by an agent and is set as its child.
    /// This affects the object's rigidbody if it has one.
    /// This method sets the "isHeld" variable to true and gives the
    /// object a "holder" that it can reference.
    /// </summary>
    /// <param name="agentInteracting">The agent interacting with the object</param>
    protected virtual void PickThisUp(GameObject agentInteracting)
    {
        //Turn off the rotation and gravity for the object.
        TurnOffRigidbody();
        //Make this object a child of the agent that picked it up.
        ChildThisToPlayerOrCamera(agentInteracting); //Sets "holder" to an object
        //Ignore collision with the agent holding this object.
        IgnoreCollisionWithPlayer(true);
        //The object is currently being held.
        isHeld = true;
    }

    /// <summary>
    /// The object is dropped.
    /// This affects the object's rigidbody if it has one.
    /// This method sets the "isHeld" variable to false and removes
    /// the current "holder" reference, setting it to "null" instead.
    /// </summary>
    protected virtual void DropThis()
    {
        //Turn on rotation and gravity for the object.
        TurnOnRigidbody();
        //Stop ignoring collision with the agent holding the object.
        IgnoreCollisionWithPlayer(false);
        //Unchild this from the agent currently holding this.
        UnchildThisFromPlayerOrCamera(); //Sets "holder" to null
        //The object is no longer being held.
        isHeld = false;
    }

    /// <summary>
    /// Turns off gravity and rotation for this object.
    /// </summary>
    private void TurnOffRigidbody()
    {
        if (thisRigidbody != null)
        {
            thisRigidbody.useGravity = false;
            thisRigidbody.freezeRotation = true;
        }
    }

    /// <summary>
    /// Turns on gravity and rotation for this object.
    /// </summary>
    private void TurnOnRigidbody()
    {
        if (thisRigidbody != null)
        {
            thisRigidbody.useGravity = true;
            thisRigidbody.freezeRotation = false;
        }
    }

    /// <summary>
    /// Ignores collision between the object and the player currently holding it.
    /// </summary>
    /// <param name="trueOrFalse">Should collision be ignored or not?</param>
    private void IgnoreCollisionWithPlayer(bool trueOrFalse)
    {
        //Make sure someone is holding it.
        if(holder != null)
        {
            Collider parentCollider = holder.GetComponent<Collider>();
            if (parentCollider == null)
            {
                //In case there is no collider on the agent holding this, look for one in its parent.
                parentCollider = holder.GetComponentInParent<Collider>();
            }
            //If a collider is found, ignore collision
            if (parentCollider != null)
            {
                Physics.IgnoreCollision(parentCollider, this.GetComponent<Collider>(), trueOrFalse);
            }
        }
    }

    /// <summary>
    /// Makes the object a child of the agent picking it up. Sets "holder" to an object.
    /// </summary>
    /// <param name="agentInteracting"></param>
    private void ChildThisToPlayerOrCamera(GameObject agentInteracting)
    {
        //ChildToPlayer is set in the editor
        if(childToPlayer)
        {
            //If specified, this object translates based on the location of the player, and not the camera.
            this.transform.parent = agentInteracting.transform;
            //Set the holder to be the player.
            holder = agentInteracting;
        }
        else
        {
            //By default, set the object as a child of the camera if there is one.
            GameObject camera = agentInteracting.GetComponentInChildren<Camera>().gameObject;
            this.transform.parent = camera.transform;
            //Set the holder to be the camera.
            holder = camera;
        }
    }

    /// <summary>
    /// Detaches the object from the agent currently holding it. Sets "holder" to null. 
    /// </summary>
    private void UnchildThisFromPlayerOrCamera()
    {
        this.transform.parent = null;
        holder = null;
    }
}
