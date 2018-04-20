using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates the phone portal after the tableau dialogue is finished.
/// </summary>
public class ActivatePhonePortal : MonoBehaviour 
{
	private bool hasPlayerTriggeredVO = false;

	private void OnEnable()
	{
		SubtitleManager.SubtitleFinished += CheckIfPortalShouldActivate;
	}

	private void OnDisable()
	{
		SubtitleManager.SubtitleFinished -= CheckIfPortalShouldActivate;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			hasPlayerTriggeredVO = true;
		}
	}

	//Check if all arguments for the portal to activate are satisfied. If they are, activate the necessary components.
	private void CheckIfPortalShouldActivate(int i)
	{
		if (hasPlayerTriggeredVO == true) 
		{
			ActivatePortal ();
		}
	}

	//Activate the necessary components for the portal to work.
	private void ActivatePortal()
	{
		foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>()) 
		{
			r.enabled = true;
		}
		GetComponentInChildren<PortalNew> ().enabled = true;
		GetComponentInChildren<GlitchValueGenerator> ().enabled = true;
		GetComponentInChildren<BoxCollider> ().enabled = true;
		GetComponentInChildren<MeshCollider> ().enabled = true; 
	}
}
