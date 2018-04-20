using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activate third portal after player has teleported 3 times.
/// </summary>
public class ActivateThirdPortal : MonoBehaviour 
{
	private int timesTeleported;

	private void OnEnable()
	{
		PortalNew.PlayerTeleported += IncrementTimesTeleported;
	}

	private void OnDisable()
	{
		PortalNew.PlayerTeleported -= CheckIfPortalShouldActivate;
	}

	private void IncrementTimesTeleported()
	{
		timesTeleported++;
		CheckIfPortalShouldActivate ();
	}

	private void CheckIfPortalShouldActivate()
	{
		if (timesTeleported >= 3) 
		{
			ActivatePortal ();
		}
	}

	//Activate necessary components for teleporter to work properly.
	private void ActivatePortal()
	{
		GetComponent<PortalNew> ().enabled = true;
		GetComponent<GlitchValueGenerator> ().enabled = true;
		GetComponentInChildren<MeshRenderer> ().enabled = true;
		GetComponentInChildren<BoxCollider> ().enabled = true;
	}
}
