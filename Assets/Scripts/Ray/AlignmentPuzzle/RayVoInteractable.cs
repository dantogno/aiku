using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class RayVoInteractable : MonoBehaviour, IInteractable
{
	public static event Action VOAudioTriggered;
	// Use this for initialization
	void Start () {
		
	}
	public virtual void Interact(GameObject agent)
	{
		if (VOAudioTriggered != null) VOAudioTriggered.Invoke();
	}
}
