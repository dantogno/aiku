using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusketState { Grab, Grapple, Scoped, Off };

public class MiningMusket : MonoBehaviour
{
    public Camera mainCamera;
	public MusketState musketState;
    public float smoothSpeed = 0.125f;
    public float grabDistance = 3.0f;
    public float rayCastDistance = 10f;
	public float grappleSpeed = 0.5f;
	public float grappleOffset = 0.75f;
	public bool isGrabbingObject = false;
	public bool hasOre = false;

	private RaycastHit hit;
	public GameObject objectToGrab;
	private Vector3 offset;
	private bool musketRaycastHit;
	private bool hasSetGrappleOffset = false;

	// Use this for initialization
	void Start ()
    {
		musketState = MusketState.Off;
	}
	
	// Update is called once per frame
	void Update ()
    {
		MiningMusketStateMachine ();
    }

    private void FixedUpdate()
    {
		Off ();
		Grab();
		Grapple ();
        MiningMusketRaycast();
    }

    void Grab()
    {
		if (musketState == MusketState.Grab) 
		{
			if (hit.transform.tag == "Metal Object") 
			{
				objectToGrab = hit.transform.gameObject;
				Rigidbody objectRigidbody = objectToGrab.GetComponent<Rigidbody> ();
				Vector3 desiredPosition = mainCamera.transform.position + mainCamera.transform.forward * grabDistance;
				Vector3 smoothedPosition = Vector3.Lerp (objectToGrab.transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

				isGrabbingObject = true;
				objectRigidbody.useGravity = false;
				objectRigidbody.transform.position = smoothedPosition;
			}

			if (hit.transform.tag == "Good Ore") 
			{
				Destroy (hit.transform.gameObject);
				hasOre = true;
				musketState = MusketState.Off;
			}
		}
    }

	void Grapple()
	{
		if (musketState == MusketState.Grapple) 
		{
			GameObject playerGameObject = this.transform.parent.gameObject.transform.parent.gameObject;
			Vector3 velocity = Vector3.zero;

			this.GetComponentInParent<Rigidbody> ().useGravity = false;
			if (!hasSetGrappleOffset) 
			{
				offset = Vector3.Lerp (playerGameObject.transform.position, hit.transform.position, grappleOffset);
				hasSetGrappleOffset = true;
			}
			playerGameObject.transform.position = Vector3.SmoothDamp (playerGameObject.transform.position, offset, ref velocity, grappleSpeed * Time.deltaTime, 35f);
		}
	}

	void Off()
	{
		if (musketState == MusketState.Off) 
		{
			if (isGrabbingObject) 
			{
				objectToGrab.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				objectToGrab.GetComponent<Rigidbody> ().useGravity = true;
				isGrabbingObject = false;
			}

			if (hasSetGrappleOffset) 
			{
				this.GetComponentInParent<Rigidbody> ().useGravity = true;
				hasSetGrappleOffset = false;
			}
		}
	}

	void MiningMusketStateMachine()
	{
		if (musketRaycastHit) 
		{
			if (hit.transform.tag == "Metal Object" && Input.GetButton ("Fire1") || hit.transform.tag == "Good Ore" && Input.GetButtonDown("Fire1")) 
			{
				musketState = MusketState.Grab;
			} 
			else if (hit.transform.tag == "Grapple" && Input.GetButton("Fire1")) 
			{
				musketState = MusketState.Grapple;
			}
			else 
			{
				musketState = MusketState.Off;
			}
		} 
		else 
		{
			if (!isGrabbingObject) 
			{
				musketState = MusketState.Off;
			}
		}
	}

    void MiningMusketRaycast()
    {
		if (!isGrabbingObject) 
		{
			Vector3 fwd = transform.TransformDirection(Vector3.forward);
			musketRaycastHit = Physics.Raycast (mainCamera.transform.position, fwd, out hit, rayCastDistance);
			Debug.DrawRay(mainCamera.transform.position, fwd * rayCastDistance, Color.green);
		}
    }
}
