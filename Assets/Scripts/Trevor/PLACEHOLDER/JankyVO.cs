using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JankyVO : MonoBehaviour 
{
	[SerializeField] private PortalNew tabletPortal;

	void OnEnable()
	{
		SubtitleManager.SubtitleFinished += SwitchBool;
	}

	void OnDisable()
	{
		SubtitleManager.SubtitleFinished -= SwitchBool;
	}

	void SwitchBool(int i)
	{
		tabletPortal.enabled = true;
        GetComponentInChildren<GlitchValueGenerator>().enabled = true;
        this.enabled = false;
	}
}
