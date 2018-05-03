using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activate first portal object after player has scoped in and picked up a crate. Should be attached to first portal game object.
/// </summary>
public class ActivateFirstPortalObject : MonoBehaviour 
{
    private int timesVOFinished = 0;

	private void OnEnable()
	{
        VOAudio.VOAudioFinished += IncrementTimesVOFinished;
	}

	private void OnDisable()
	{
        VOAudio.VOAudioFinished -= IncrementTimesVOFinished;
	}

    /// <summary>
    /// Counts how many times a VO has played. It has played enough times, activate the portal.
    /// </summary>
    private void IncrementTimesVOFinished()
    {
        timesVOFinished++;

        if(timesVOFinished >= 2)
        {
            ActivatePortalObject();
        }
    }

	/// <summary>
	/// Activate the necessary components for the portal to work. Disables this script
	/// </summary>
	private void ActivatePortalObject()
	{
		GetComponent<PortalNew> ().enabled = true;
		GetComponent<GlitchValueGenerator> ().enabled = true;
		GetComponentInChildren<MeshRenderer> ().enabled = true;
        this.enabled = false;
	}
}
