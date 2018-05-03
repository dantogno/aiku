using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCaveAudio : MonoBehaviour
{
    [SerializeField] private AudioSource caveAmbience;
    [SerializeField] private AudioSource caveMusic;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!caveAmbience.isPlaying)
            {
                caveAmbience.Play();
                caveMusic.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            caveAmbience.Stop();
            caveMusic.Stop();
        }
    }
}
