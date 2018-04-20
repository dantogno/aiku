using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles audio trigger events.
/// The script can be applied to any trigger collider.
/// </summary>

public class AudioTrigger : MonoBehaviour
{
    public static event Action PlayerEnteredTrigger;

    private AudioSource myAudioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // Play the audio.
            myAudioSource = GetComponent<AudioSource>();
            myAudioSource.Play();

            // Disable the trigger.
            GetComponent<Collider>().enabled = false;

            // Broadcast an event, in case any other script is interested to know if the player entered an audio trigger.
            if (PlayerEnteredTrigger != null) PlayerEnteredTrigger.Invoke();
        }
    }
}
