using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubBotVoiceManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] botVoices;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") EnableAudioSources();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") DisableAudioSources();
    }

    private void EnableAudioSources()
    {
        foreach (AudioSource a in botVoices) a.volume = 1;
    }

    private void DisableAudioSources()
    {
        foreach (AudioSource a in botVoices) a.volume = 0;
    }
}
