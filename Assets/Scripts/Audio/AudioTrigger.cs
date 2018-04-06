using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    private AudioSource myAudioSource;

    private void OnTriggerEnter(Collider other)
    {
        myAudioSource = GetComponent<AudioSource>();

        myAudioSource.Play();

        GetComponent<Collider>().enabled = false;
    }
}
