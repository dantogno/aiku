using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates the VO that tells the player how to grapple.
/// </summary>
public class ActivateGrappleVO : MonoBehaviour 
{
	private int timesTeleported;
	private bool hasEnteredTrigger = false;

	private void OnEnable()
	{
		PortalNew.PlayerTeleported += IncrementTimesTeleported;
	}

	private void OnDisable()
	{
		PortalNew.PlayerTeleported -= IncrementTimesTeleported;
	}

	/// <summary>
	/// Increments the times teleported every time the player teleports.
	/// </summary>
	private void IncrementTimesTeleported()
	{
		timesTeleported++;
	}

	/// <summary>
	/// Calls the TriggerVOAudio function from the VOAudio script in the child object.
	/// </summary>
	private void ActivateVO()
	{
        if(GetComponentInChildren<VOAudio>() != null)
            GetComponentInChildren<VOAudio> ().TriggerVOAudio ();
        if(GetComponent<BoxCollider>() != null)
            GetComponent<BoxCollider>().enabled = false;
        this.enabled = false;
	}

	/// <summary>
	/// Sets hasEnteredTrigger value to true when player enters the trigger area.
	/// </summary>
	/// <param name="other">Other.</param>
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			hasEnteredTrigger = true;
		}

		if (timesTeleported == 3 && hasEnteredTrigger) 
		{
			ActivateVO ();
		}
	}

	/// <summary>
	/// Sets hasEnteredTrigger value to false when player exits the trigger area.
	/// </summary>
	/// <param name="other">Other.</param>
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			hasEnteredTrigger = false;
		}
	}
}
