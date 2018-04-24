using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates second portal once player picks up the crystal.
/// </summary>
public class ActivateSecondPortal : MonoBehaviour 
{
	private void OnEnable()
	{
		CrystalPickUp.ActivateSecondPortal += ActivatePortal;
	}

	private void OnDisable()
	{
		CrystalPickUp.ActivateSecondPortal -= ActivatePortal;
	}

	/// <summary>
	/// Activates the necessary components in order for the player to use the portal.
	/// </summary>
	private void ActivatePortal()
	{
		foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>()) 
		{
			r.enabled = true;
		}
		foreach (MeshCollider c in GetComponentsInChildren<MeshCollider>()) 
		{
			c.enabled = true;
		}
		GetComponent<PortalNew> ().enabled = true;
		GetComponent<GlitchValueGenerator> ().enabled = true;
	}
}
