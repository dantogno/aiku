using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GMDState { Grab, Mining, Grapple, Scoped, Off };

/// <summary>
/// Functionality for the GMD (Geological Manipulation Device). 
/// The player can use this to interact with objects(Picking up objects, grappling to objects, mining objects, etc.). 
/// Should be attached to the GMD GameObject.
/// </summary>
public class GMD : MonoBehaviour
{
	[Tooltip("The 'Player' GameObject goes here. Must have a Rigidbody component.")] 
	[SerializeField] private GameObject playerGameObject;

	[Tooltip("Camera used by the player.")]
	[SerializeField] private Camera playerCamera;

	[Tooltip("Current state of the GMD. Off means it is not currently doing anything.")]
	[SerializeField] private GMDState currentState;

	[Tooltip("Determines how quickly an object that is picked up with the GMD follows the player's camera.")]
	[SerializeField] private float smoothSpeed = 0.125f;

	[Tooltip("Object's distance from player when picked up by the GMD.")]
	[SerializeField] private float grabDistance = 3.0f;

	[Tooltip("Distance for detecting objects.")]
	[SerializeField] private float rayCastDistance = 10f;

	[Tooltip("How quickly the player moves when grappling.")]
	[SerializeField] private float grappleSpeed = 0.5f;

	[Tooltip("Percent value for how far a player will grapple. 0.75 = Player will go 75% of the way to the grapple object from where the player is standing.")]
	[SerializeField] private float grappleOffset = 0.75f;

    [Tooltip("Grab sound for the GMD located in Audio/Trevor/Sound Effects. It is called MagnetGunRough.")]
    [SerializeField] private AudioClip grabSound;

	public bool isGrabbingObject = false;

	public static event Action<int> PickupObject;
    public static event Action MiningStart;
	public static event Action MiningEnd;

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

	private bool hasSentEvent = false;
    private AudioSource audioSource;

	// Use this for initialization
	private void Start ()
	{
		currentState = GMDState.Off;
		playerRigidbody = playerGameObject.GetComponent<Rigidbody> ();
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
    }

	private void FixedUpdate()
	{
		GMDStateMachine ();
		GMDRaycast();
	}

	/// <summary>
	/// Sets the state of the GMD to the new state.
	/// </summary>
	/// <param name="state">State.</param>
	private void SetCurrentState(GMDState state)
	{
		currentState = state;
	}

	/// <summary>
	/// Determines when each state should be active.
	/// </summary>
	private void GMDStateMachine()
	{
		switch (currentState) 
		{
		//If the GMD is in the Grab state, calls the Grab function. Otherwise, switch to the Off state.
		case GMDState.Grab:
			
			Grab ();
			
			if (!Input.GetButton ("Interact")) 
			{
				SetCurrentState (GMDState.Off);
			}
			break;

		//If the player is looking at a grapple object and pressing the interact button, call the Grapple function. Otherwise, switch to the Off state.
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

		//If the GMD is in the Mining state, calls the Mining function. Otherwise, switch to the Off state.
		case GMDState.Mining:
			if (!Input.GetButton ("Interact")) 
			{
				if (MiningEnd != null)
					MiningEnd.Invoke ();
				SetCurrentState (GMDState.Off);
			}
			break;	

		//If the GMD is in the Off state, calls the Off function. Switches to other functions according to what the player is interacting with.
		case GMDState.Off:
			
			Off ();
			
			if (hit.transform != null) 
			{
				if (hit.transform.tag == "Metal Object" && Input.GetButton ("Interact")) 
				{
					SetCurrentState (GMDState.Grab);
				} 
				else if (hit.transform.tag == "Good Ore" && Input.GetButton ("Interact")) 
				{
					SetCurrentState (GMDState.Mining);
					if (MiningStart != null)
						MiningStart.Invoke ();
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
	private void GMDRaycast()
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
	private void Grab()
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
				if (!hasSentEvent) 
				{
					if (PickupObject != null) 
					{
						PickupObject.Invoke (0);
					}
					hasSentEvent = true;
				}
                if (grabSound != null && !audioSource.isPlaying)
                {
                    audioSource.clip = grabSound;
                    audioSource.Play();
                }
			}
		}
	}

    /// <summary>
    /// Turns off gravity on player rigidbody and uses Vector3.SmoothDamp to move player smoothly toward object tagged as 'Grapple'.
    /// </summary>
    private void Grapple()
	{
		playerRigidbody.useGravity = false;
		if (!hasSetGrappleOffset) 
		{
			offset = Vector3.Lerp (playerGameObject.transform.position, hit.transform.position, grappleOffset);
			hasSetGrappleOffset = true;
		}
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }

		Vector3 velocity = Vector3.zero;
		playerGameObject.transform.position = Vector3.SmoothDamp (playerGameObject.transform.position, offset, ref velocity, grappleSpeed * Time.deltaTime, 35f);
	}

	/// <summary>
	/// Resets gravity and velocity for grabbed object. Resets gravity for player that was previously grappling to an object.
	/// </summary>
	private void Off()
	{
		if (isGrabbingObject) 
		{
			if (objectToGrab != null) 
			{
				Rigidbody objectRigidbody = objectToGrab.GetComponent<Rigidbody> ();
				objectRigidbody.velocity = Vector3.zero;
				objectRigidbody.useGravity = true;
				isGrabbingObject = false;
				hasSentEvent = false;
                audioSource.Stop();
			}
		}

		if (hasSetGrappleOffset) 
		{
			playerRigidbody.useGravity = true;
			hasSetGrappleOffset = false;
            audioSource.Stop();
		}
	}
}
