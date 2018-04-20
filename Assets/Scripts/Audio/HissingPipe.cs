using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls the audio of the broken pipe in the engine room.
/// The script is applied to the steam particle effect child of the broken pipe.
/// </summary>

public class HissingPipe : MonoBehaviour
{
    [SerializeField, Tooltip("This clip is the looping steam hiss.")]
    private AudioClip steamClip;

    [SerializeField, Tooltip("The pipe starts hissing when this task is complete.")]
    private Task fuelCleaningTask;

    [SerializeField, Tooltip("Valves to turn. Each valve quiets down the steam a little.")]
    private EngineRoomValve[] valves;

    [SerializeField, Tooltip("The amount by which to decrease the volume and pitch with each valve closed.")]
    private float volumeDeduction = .3f;

    private AudioSource myAudioSource;

    private void Awake()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        // When the burst pipe GameObject is enabled, we start monitoring when the hissing steam sound should be cued.
        CallClipChangerCoroutine();
        foreach (EngineRoomValve valve in valves) valve.ClosedValve += QuietDown;
    }
    private void OnDisable()
    {
        foreach (EngineRoomValve valve in valves) valve.ClosedValve -= QuietDown;
    }

    /// <summary>
    /// The steam sound must loop as soon as the pipe bursting sound is complete.
    /// </summary>
    private void CallClipChangerCoroutine()
    {
        StartCoroutine(WaitForAudioClipToFinishAndSwitchAudioClip());
    }

    /// <summary>
    /// The pipe noise quiets down a little bit each time a valve is turned.
    /// </summary>
    private void QuietDown()
    {
        myAudioSource.volume -= volumeDeduction;
        myAudioSource.pitch -= volumeDeduction;
    }

    /// <summary>
    /// After the audio source finishes playing its clip, loop the steam hissing clip.
    /// </summary>
    private IEnumerator WaitForAudioClipToFinishAndSwitchAudioClip()
    {
        while (myAudioSource.isPlaying) yield return null;

        myAudioSource.clip = steamClip;
        myAudioSource.loop = true;
        myAudioSource.Play();
    }
}
