using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Disable ceiling tiles after player has teleported twice. This should go on the ceiling tiles parent gameobject.
/// </summary>
public class DisableCeilingTiles : MonoBehaviour 
{
	private int timesTeleported;

	private void OnEnable()
	{
		PortalNew.PlayerTeleported += IncremementTimesTeleported;
	}

	private void OnDisable()
	{
		PortalNew.PlayerTeleported -= IncremementTimesTeleported;
	}

	/// <summary>
	/// Incremements the times teleported every time the player teleports;
	/// </summary>
	private void IncremementTimesTeleported()
	{
		timesTeleported++;
		CheckIfShouldDisableTiles ();
	}

	/// <summary>
	/// Checks if should disable tiles every time the player teleports.
	/// </summary>
	private void CheckIfShouldDisableTiles()
	{
		if (timesTeleported >= 2) 
		{
			DisableTiles ();
		}
	}

	/// <summary>
	/// Disables the tiles.
	/// </summary>
	private void DisableTiles()
	{
		this.gameObject.SetActive (false);
	}
}
