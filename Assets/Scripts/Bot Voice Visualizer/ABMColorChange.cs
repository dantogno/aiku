using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Switches the color showing on-screen during an audio clip to show multiple AI voices speaking through the same screen
/// </summary>
public class ABMColorChange : AmplitudeBrightnessModulation {

    [SerializeField]
    [Tooltip("The audio source that will play the clip to change colors during")]
    private AudioSource audioSource;

    [SerializeField]
    [Tooltip("Time after the source begins playing to switch colors")]
    private float waitTime;

    [SerializeField]
    private Color color1, color2;

    private bool duringClip = false;
    private float timeClipStarted = 0;
	
	// Update is called once per frame
	override protected void Update () {
	    if(audioSource.isPlaying && !duringClip)
        {
            duringClip = true;
            timeClipStarted = Time.time;
            color = color1;
        }

        if(!audioSource.isPlaying && duringClip)
        {
            duringClip = false;
        }

        if(duringClip && Time.time > timeClipStarted + waitTime)
        {
            color = color2;
        }

        base.Update();
	}
}
