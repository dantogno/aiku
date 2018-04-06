using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Switches the color showing on-screen during an audio clip to show multiple AI voices speaking through the same screen.
/// If multiple bot voices talk during a single audio clip and you want them to show up on the same screen,
/// use this class to switch the background color of the screen when the second bot begins talking
/// </summary>
public class ABMColorChange : AmplitudeBrightnessModulation
{
    [SerializeField]
    [Tooltip("The audio source that will play the clip to change colors during")]
    private AudioSource audioSource;

    [SerializeField]
    [Tooltip("Time after the source begins playing that the second bot begins to speak, or the time to switch the background color from Color 1 to Color 2")]
    private float waitTime;

    [SerializeField]
    [Tooltip("Color 1 is the first bot's color, and Color 2 is the color to switch in when the second bot begins talking")]
    private Color color1, color2;

    private bool duringClip = false;
    private float timeClipStarted = 0;
	
	// Update is called once per frame
	override protected void Update ()
    {
        //when we first detect that the clip is playing
	    if(audioSource.isPlaying && !duringClip)
        {
            duringClip = true;
            timeClipStarted = Time.time;
            color = color1;
        }

        //when we first detect that the clip has ended / stopped playing
        if(!audioSource.isPlaying && duringClip)
        {
            duringClip = false;
        }

        //if the clip is playing and waitTime has elapsed, time to switch colors
        if(duringClip && Time.time > timeClipStarted + waitTime)
        {
            color = color2;
        }

        base.Update();
	}
}
