using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSceneTransition : MonoBehaviour 
{
	private void OnEnable()
	{
		
	}

	private void OnDisable()
	{
		
	}

	private void ActivateTransitionScript()
	{
		GetComponentInChildren<SceneTransition> ().enabled = true;
	}
}
