using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Allows the GMD to be picked up by the player. Put this script on the GMD game object.
/// </summary>
public class PickupGMD : MonoBehaviour, IInteractable
{
    public static event Action PickedUpGMD;

    /// <summary>
    /// Update is needed to enable/disable script
    /// </summary>
    private void Update()
    {
        
    }

    //Sets the GMD's parent object to be the player, moves the GMD to the correct position, and turns on/off the necessary components.
    void IInteractable.Interact(GameObject agent)
	{
        if (PickedUpGMD != null) PickedUpGMD.Invoke();

        GameObject gmdObject = this.gameObject;
		Transform gmdObjectTransform = gmdObject.transform;

		gmdObjectTransform.SetParent (agent.transform.GetChild (0));
		gmdObjectTransform.localPosition = new Vector3 (0.6003374f, -0.55f, 0.8120064f);
		gmdObjectTransform.localRotation = Quaternion.Euler (0, 0, 0);
		gmdObject.GetComponent<BoxCollider> ().enabled = false;

        GameObject.Find("Scanning Camera").SetActive(false);
	}
}
