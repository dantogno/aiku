using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class VOAudio : MonoBehaviour {

    [Tooltip("Enter subtitles here. Separate each subtitle with a tilde, like this.~ And this.~")]
    [SerializeField, TextArea(3, 5)] string Subtitle;
    AudioSource audioSource;

    public static Action<string> VOAudioTriggered;   

	// Use this for initialization
	void Start () {
        audioSource = this.GetComponent<AudioSource>();
	}

    /// <summary>
    /// Plays the audio file and passes the subtitles to be shown by SubtitleManager
    /// </summary>
    public void TriggerVOAudio()
    {
        audioSource.Play();
        if (VOAudioTriggered != null)
            VOAudioTriggered.Invoke(Subtitle); 
    }

    /// <summary>
    /// Alternatively, VOAudio can be triggered by an attached trigger volume
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.tag == "Player") 
		{
			TriggerVOAudio();
			this.GetComponent<BoxCollider> ().enabled = false;
		}
    }
}
