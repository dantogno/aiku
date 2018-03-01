using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GMDState { Grab, Grapple, Scoped, Off };

/// <summary>
/// Functionality for the GMD. Should be attached to the GMD GameObject.
/// </summary>
public class GMD : MonoBehaviour
{
	[Tooltip("The 'Player' GameObject goes here. Must have a Rigidbody component.")] 
	[SerializeField] GameObject playerGameObject;

	[Tooltip("Camera used by the player.")]
	[SerializeField] Camera playerCamera;

	[Tooltip("Current state of the GMD. Off means it is not currently doing anything.")]
	[SerializeField] GMDState currentState;

	[Tooltip("Determines how quickly an object that is picked up with the GMD follows the player's camera.")]
	[SerializeField] float smoothSpeed = 0.125f;

	[Tooltip("Object's distance from player when picked up by the GMD.")]
	[SerializeField] float grabDistance = 3.0f;

	[Tooltip("Distance for detecting objects.")]
	[SerializeField] float rayCastDistance = 10f;

	[Tooltip("How quickly the player moves when grappling.")]
	[SerializeField] float grappleSpeed = 0.5f;

	[Tooltip("Percent value for how far a player will grapple. 0.75 = Player will go 75% of the way to the grapple object from where the player is standing.")]
	[SerializeField] float grappleOffset = 0.75f;

	[Tooltip("Can see if player is grabbing an object. Currently used by another script but that will most likely change and this variable will become private.")]
	public bool isGrabbingObject = false;

	[Tooltip("Allows other scripts to check if player has the ore. This is temporary and will be trashed. Ignore.")]
	public bool hasOre = false;

	//This allows the GMD to continue to hold the object while the interact button is down even when the raycast is not pointed at the object. 
	private GameObject objectToGrab;
	
	//Used in the grapple function to normalize the distance between the player and the grapple object and give distance percentage for moving the player.
	private Vector3 offset;
	
	//Used so that grappleOffset value is not overwritten after it is set.
	private bool hasSetGrappleOffset = false;

	//Object that is hit by raycast.
	private RaycastHit hit;

	//Returns true if raycast hits an object. 
	private bool gmdRaycastHit;

	//Rigidbody of the player.
	private Rigidbody playerRigidbody;

	// Use this for initialization
	void Start ()
	{
		currentState = GMDState.Off;
		playerRigidbody = playerGameObject.GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update ()
	{
		GMDStateMachine ();
	}

	private void FixedUpdate()
	{
		GMDRaycast();
	}

	/// <summary>
	/// Sets the state of the GMD to the new state.
	/// </summary>
	/// <param name="state">State.</param>
	void SetCurrentState(GMDState state)
	{
		currentState = state;
	}

	/// <summary>
	/// Determines when each state should be active.
	/// </summary>
	void GMDStateMachine()
	{
		switch (currentState) 
		{
		case GMDState.Grab:
			
			Grab ();
			
			if (!Input.GetButton ("Interact")) 
			{
				SetCurrentState (GMDState.Off);
			}
			break;
			
		case GMDState.Grapple:
			
			if (gmdRaycastHit && hit.transform.tag == "Grapple" && Input.GetButton ("Interact")) 
			{
				Grapple ();
			} 
			else 
			{
				SetCurrentState (GMDState.Off);
			}
			break;
			
		case GMDState.Off:
			
			Off ();
			
			if (gmdRaycastHit) 
			{
				if (hit.transform.tag == "Metal Object" && Input.GetButton ("Interact") || hit.transform.tag == "Good Ore" && Input.GetButton ("Interact")) 
				{
					SetCurrentState (GMDState.Grab);
				} 
				else if (hit.transform.tag == "Grapple" && Input.GetButton ("Interact")) 
				{
					SetCurrentState (GMDState.Grapple);
				}
			}
			break;
		}
	}

	/// <summary>
	/// Raycasts in front of the player camera if the player is not grabbing an object.
	/// </summary>
	void GMDRaycast()
	{
		if (!isGrabbingObject) 
		{
			Vector3 forward = transform.TransformDirection(Vector3.forward);
			gmdRaycastHit = Physics.Raycast (playerCamera.transform.position, forward, out hit, rayCastDistance);
			
			#if UNITY_EDITOR
			Debug.DrawRay(playerCamera.transform.position, forward * rayCastDistance, Color.green);
			#endif
		}
	}

	/// <summary>
	/// Smoothly lerps object to a specific distance in front of the player camera.
	/// </summary>
	void Grab()
	{
		if (hit.transform.tag == "Metal Object") 
		{
			objectToGrab = hit.transform.gameObject;
			Rigidbody objectRigidbody = objectToGrab.GetComponent<Rigidbody> ();
			Vector3 desiredPosition = playerCamera.transform.position + playerCamera.transform.forward * grabDistance;
			Vector3 smoothedPosition = Vector3.Lerp (objectToGrab.transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

			if (objectToGrab != null) 
			{
				isGrabbingObject = true;
				objectRigidbody.useGravity = false;
				objectRigidbody.transform.position = smoothedPosition;
			}
		}

		//This is a temporary way for the player to pick up ore. This will change.
		if (hit.transform.tag == "Good Ore") 
		{
			Destroy (hit.transform.gameObject);
			hasOre = true;
			currentState = GMDState.Off;
		}
	}

	/// <summary>
	/// Turns off gravity on player rigidbody and uses Vector3.SmoothDamp to move player smoothly toward object tagged as 'Grapple'.
	/// </summary>
	void Grapple()
	{
		playerRigidbody.useGravity = false;
		if (!hasSetGrappleOffset) 
		{
			offset = Vector3.Lerp (playerGameObject.transform.position, hit.transform.position, grappleOffset);
			hasSetGrappleOffset = true;
		}

		Vector3 velocity = Vector3.zero;
		playerGameObject.transform.position = Vector3.SmoothDamp (playerGameObject.transform.position, offset, ref velocity, grappleSpeed * Time.deltaTime, 35f);
	}

	/// <summary>
	/// Resets gravity and velocity for grabbed object. Resets gravity for player that was previously grappling to an object.
	/// </summary>
	void Off()
	{
		if (isGrabbingObject) 
		{
			if (objectToGrab != null) 
			{
				Rigidbody objectRigidbody = objectToGrab.GetComponent<Rigidbody> ();
				objectRigidbody.velocity = Vector3.zero;
				objectRigidbody.useGravity = true;
				isGrabbingObject = false;
			}
		}

		if (hasSetGrappleOffset) 
		{
			playerRigidbody.useGravity = true;
			hasSetGrappleOffset = false;
		}
	}
}
