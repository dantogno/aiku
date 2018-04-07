using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateFirstPortal : MonoBehaviour 
{
	[SerializeField] private GameObject portalGameObject;

    private bool hasTriggeredVO = false;

	void OnEnable()
	{
		GMD.PickupObject += ActivatePortal;
	}

	void OnDisable()
	{
		GMD.PickupObject -= ActivatePortal;
	}

	void ActivatePortal(int i)
	{
		if (!hasTriggeredVO) 
		{
			portalGameObject.SetActive (true);
			this.GetComponent<VOAudio> ().TriggerVOAudio ();
            hasTriggeredVO = true;
		}
	}
}
