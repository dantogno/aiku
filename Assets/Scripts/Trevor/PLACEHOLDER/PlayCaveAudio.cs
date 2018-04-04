using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCaveAudio : MonoBehaviour
{
    [SerializeField] private AudioSource caveAudio;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!caveAudio.isPlaying)
            {
                caveAudio.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            caveAudio.Stop();
        }
    }
}
