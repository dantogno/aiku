using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class plays an audio clip when a task is complete.
/// This is a flexible script in terms of application; it can be added to task objects, objects with AudioSource components, or a dedicated manager object.
/// It is currently applied to task objects.
/// </summary>

public class AudioCue : MonoBehaviour
{
    // Options for audio responses.
    private enum Cue { PlaySound, StopSound, FadeInSound, FadeOutSound, IncreasePitch, DecreasePitch }

    [SerializeField, Tooltip("The audio response which will happen when a task is complete.")]
    private Cue cue;

    [SerializeField, Tooltip("The task which will trigger an audio response.")]
    private Task task;

    [SerializeField, Tooltip("The AudioSource which will respond to the cue.")]
    private AudioSource audioSource;

    [SerializeField, Tooltip("The clip which will be played, if applicable.")]
    private AudioClip clip;

    [SerializeField, Tooltip("The amount of time to wait to execute the cue, if applicable.")]
    private float waitTime = 0;

    [SerializeField, Tooltip("The amount of time over which audio will fade, if applicable.")]
    private float fadeTimer = 3;

    [SerializeField, Tooltip("The desired volume, if fading audio in.")]
    private float targetVolume = 1;

    [SerializeField, Tooltip("The desired pitch to increase, if increasing pitch.")]
    private float targetPitchModifier = 1;

    private void Awake()
    {
        if (task == null) task = GetComponent<Task>();
    }

    private void OnEnable()
    {
        task.OnTaskCompleted += CheckCue;
    }
    private void OnDisable()
    {
        task.OnTaskCompleted -= CheckCue;
    }

    /// <summary>
    /// Trigger a different audio response, depending on the chosen cue.
    /// </summary>
    private void CheckCue()
    {
        switch (cue)
        {
            case Cue.PlaySound:
                StartCoroutine(PlaySound());
                break;
            case Cue.StopSound:
                StartCoroutine(StopSound());
                break;
            case Cue.FadeInSound:
                StartCoroutine(FadeInSound());
                break;
            case Cue.FadeOutSound:
                StartCoroutine(FadeOutSound());
                break;
            case Cue.IncreasePitch:
                StartCoroutine(IncreasePitch());
                break;
            case Cue.DecreasePitch:
                StartCoroutine(DecreasePitch());
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Play an audio clip.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(waitTime);

        if (clip != null) audioSource.clip = clip;
        audioSource.Play();
    }

    /// <summary>
    /// Stop an audio clip.
    /// </summary>
    /// <returns></returns>
    private IEnumerator StopSound()
    {
        yield return new WaitForSeconds(waitTime);
        
        audioSource.Stop();
    }

    /// <summary>
    /// Fade an audio clip in.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeInSound()
    {
        float originalVolume = audioSource.volume;

        float elapsedTime = 0;
        while (elapsedTime < fadeTimer)
        {
            audioSource.pitch = Mathf.Lerp(originalVolume, targetVolume, elapsedTime / fadeTimer);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        audioSource.pitch = targetVolume;
    }

    /// <summary>
    /// Fade the audio clip out.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeOutSound()
    {
        float originalVolume = audioSource.volume;

        float elapsedTime = 0;
        while (elapsedTime < fadeTimer)
        {
            audioSource.volume = Mathf.Lerp(originalVolume, 0, elapsedTime / fadeTimer);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        audioSource.volume = 0;
        audioSource.Stop();
    }

    /// <summary>
    /// Increase the audio's pitch.
    /// </summary>
    /// <returns></returns>
    private IEnumerator IncreasePitch()
    {
        float originalPitch = audioSource.pitch,
            targetPitch = audioSource.pitch + targetPitchModifier;

        float elapsedTime = 0;
        while (elapsedTime < fadeTimer)
        {
            audioSource.pitch = Mathf.Lerp(originalPitch, targetPitch, elapsedTime / fadeTimer);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        audioSource.pitch = targetPitch;
    }

    /// <summary>
    /// Decrease the audio's pitch to zero.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DecreasePitch()
    {
        float originalPitch = audioSource.pitch;

        float elapsedTime = 0;
        while (elapsedTime < fadeTimer)
        {
            audioSource.pitch = Mathf.Lerp(originalPitch, 0, elapsedTime / fadeTimer);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        audioSource.pitch = 0;
        audioSource.Stop();
    }
}
