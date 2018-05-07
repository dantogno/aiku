using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaVORandomizer : MonoBehaviour {

    [SerializeField]
    private AudioSource audioSource;

    public AudioClip[] RoombaLines;

    // Use this for initialization
    void Start () {
		
        }
	
    public void RandomizeAudioSource()
    {
        audioSource.clip = RoombaLines[Random.Range(0, RoombaLines.Length)];
    }

    public void PlayAudio()
    {
        RandomizeAudioSource();
        audioSource.Play();
    }
}
