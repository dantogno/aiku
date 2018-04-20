using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Disables the cave wall after the player has teleported 3 times. Should be placed on the cave wall object (Currently named 'flatstone_floor').
/// </summary>
public class CaveWall : MonoBehaviour 
{
	private int timesTeleported = 0;

	private void OnEnable()
	{
		PortalNew.PlayerTeleported += IncremementTimesTeleportedValue;
	}

	private void IncremementTimesTeleportedValue()
	{
		timesTeleported++;
		CheckIfCaveWallShouldBeDisabled ();
	}

	private void CheckIfCaveWallShouldBeDisabled()
	{
		if (timesTeleported >= 3) 
		{
			DisableCaveWall ();
		}
	}

	private void DisableCaveWall()
	{
		this.gameObject.SetActive (false);
	}
}
