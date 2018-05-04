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
    //Toggle this bool if you want to trigger the sound on Enter. 
    [SerializeField]
    private bool willTriggerOnEnter;
    //Toggle this bool if you want to trigger the sound on Exit. 
    [SerializeField]
    private bool willTriggeronExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && willTriggerOnEnter)
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
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && willTriggeronExit)
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
