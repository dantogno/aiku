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

	/// <summary>
	/// Increments the times teleported every every time the player teleports.
	/// </summary>
	private void IncrementTimesTeleported()
	{
		timesTeleported++;
		CheckIfPortalShouldActivate ();
	}

	/// <summary>
	/// If player has teleported 3 times, call the ActivatePortal function.
	/// </summary>
	private void CheckIfPortalShouldActivate()
	{
		if (timesTeleported >= 3) 
		{
			ActivatePortal ();
		}
	}

	/// <summary>
	/// Activate necessary components for teleporter to work properly.
	/// </summary>
	private void ActivatePortal()
	{
		GetComponent<PortalNew> ().enabled = true;
		GetComponent<GlitchValueGenerator> ().enabled = true;
		GetComponentInChildren<MeshRenderer> ().enabled = true;
		GetComponentInChildren<BoxCollider> ().enabled = true;
	}
}
