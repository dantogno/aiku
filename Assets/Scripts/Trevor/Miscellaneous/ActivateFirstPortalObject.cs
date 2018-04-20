using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activate first portal object after player has scoped in and picked up a crate. Should be attached to first portal game object.
/// </summary>
public class ActivateFirstPortalObject : MonoBehaviour 
{
	private bool hasPickedUpCrate = false;
	private bool hasScopedIn = false;
	private bool portalHasBeenActivated = false;

	private void OnEnable()
	{
		GMD.PickupObject += SetHasPickedUpCrateValue;
		Scope.ScopedIn += SetHasScopedInValue;
	}

	private void OnDisable()
	{
		GMD.PickupObject -= SetHasPickedUpCrateValue;
		Scope.ScopedIn -= SetHasScopedInValue;
	}
		
	private void SetHasPickedUpCrateValue(int i)
	{
		if (hasPickedUpCrate == false) 
		{
			hasPickedUpCrate = true;
			CheckIfPortalShouldActivate ();
		}
	}

	private void SetHasScopedInValue(int i)
	{
		if (hasScopedIn == false) 
		{
			hasScopedIn = true;
			CheckIfPortalShouldActivate ();
		}
	}

	//Check if all arguments for the portal to activate are satisfied. If they are, activate the necessary components.
	private void CheckIfPortalShouldActivate()
	{
		if (hasPickedUpCrate == true && hasScopedIn == true && portalHasBeenActivated == false) 
		{
			ActivatePortalObject ();
		}
	}

	//Activate the necessary components for the portal to work.
	private void ActivatePortalObject()
	{
		GetComponent<PortalNew> ().enabled = true;
		GetComponent<GlitchValueGenerator> ().enabled = true;
		GetComponentInChildren<MeshRenderer> ().enabled = true;
		portalHasBeenActivated = true;
	}
}
