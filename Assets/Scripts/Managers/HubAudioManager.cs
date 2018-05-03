using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class currently exists as a fix for a bug wherein the bot voices play suddenly when re-entering the hub level.
/// Apply this script to a dedicated GameObject alongside any other managers, so that it is easy to find.
/// </summary>

public class HubAudioManager : MonoBehaviour
{
    [SerializeField, Tooltip("The AudioSource components attached to all the bot voice visualizer GameObjects.")]
    private AudioSource[] botVoices;

    private void OnEnable()
    {
        SceneTransition.SceneChangeStarted += DisableBotVoices;
    }
    private void OnDisable()
    {
        SceneTransition.SceneChangeStarted -= DisableBotVoices;
    }

    /// <summary>
    /// Turn off the bot voices, we don't need to hear them when we enter a level.
    /// </summary>
    private void DisableBotVoices()
    {
        foreach(AudioSource a in botVoices)
        {
            if(a != null)
            {
                a.enabled = false;
            }
        }
    }
}
