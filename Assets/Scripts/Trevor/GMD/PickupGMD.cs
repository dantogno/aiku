using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Allows the GMD to be picked up by the player. Put this script on the GMD game object.
/// </summary>
public class PickupGMD : MonoBehaviour, IInteractable
{
    [Tooltip("The canvas object of the pointer arrow that points to the GMD on the ground.")]
    [SerializeField] private GameObject Arrow;

    //Sets the GMD's parent object to be the player, moves the GMD to the correct position, and turns on/off the necessary components.
    void IInteractable.Interact(GameObject agent)
	{
		GameObject gmdObject = this.gameObject;
		Transform gmdObjectTransform = gmdObject.transform;

		gmdObjectTransform.SetParent (agent.transform.GetChild (0));
		gmdObjectTransform.localPosition = new Vector3 (0.6003374f, -0.55f, 0.8120064f);
		gmdObjectTransform.localRotation = Quaternion.Euler (0, 0, 0);
		gmdObject.GetComponent<GMD> ().enabled = true;
		gmdObject.GetComponent<Scope> ().enabled = true;
		gmdObject.GetComponent<Animator> ().enabled = true;
		gmdObject.GetComponent<BoxCollider> ().enabled = false;
		gmdObject.GetComponent<VOAudio> ().TriggerVOAudio ();
        Arrow.SetActive(false);

        GameObject.Find("Scanning Camera").SetActive(false);
	}
}
