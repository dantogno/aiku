using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayInteriorMusic : MonoBehaviour
{
    [SerializeField] private AudioSource interiorMusic;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!interiorMusic.isPlaying)
            {
                interiorMusic.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            interiorMusic.Stop();
        }
    }
}