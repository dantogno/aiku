using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls when the music plays in the hub level. It is distinct from the HubAudioManager class, which manages other audio functionality.
/// Apply this script to a dedicated GameObject alongside any other managers, so that it is easy to find.
/// </summary>

public class HubMusicManager : MonoBehaviour
{
    [SerializeField, Tooltip("These are the audio source components playing the music.")]
    private AudioSource ambientSource, ambientSource2, nonAmbientSource, titleSource;

    [SerializeField, Tooltip("How long to wait until the song starts playing.")]
    private float waitTime = 7;

    // The AudioSource's default audio clip.
    private AudioClip originalClip;

    private float originalAmbienceVolume, originalNonAmbientVolume, originalTitleVolume;

    private void Awake()
    {
        originalAmbienceVolume = ambientSource.volume;
        originalNonAmbientVolume = nonAmbientSource.volume;
        originalTitleVolume = titleSource.volume;
    }

    private void OnEnable()
    {
        EngineSequenceManager.OnShutdown += PlayMusic;
        EndingScreen.AllocatedAllShipboardPowerToCryochambers += StopMusicOnly;
        EndCredits.CreditsStarted += PlayEndCreditsMusic;
        AudioTrigger.PlayerEnteredTrigger += StopAmbience;
        SceneTransition.SceneChangeFinished += PlayAmbience;
    }
    private void OnDisable()
    {
        EngineSequenceManager.OnShutdown -= PlayMusic;
        EndingScreen.AllocatedAllShipboardPowerToCryochambers -= StopMusicOnly;
        EndCredits.CreditsStarted -= PlayEndCreditsMusic;
    }

    private void Start()
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(FadeInAmbience());
    }

    /// <summary>
    /// Ambience fades in gradually.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeInAmbience()
    {
        ambientSource.volume = 0;

        yield return new WaitForSeconds(4);

        #region Lerp ambience to preferred volume.

        float elapsedTime = 0, timer = 10;
        while (elapsedTime < timer)
        {
            ambientSource.volume = Mathf.Lerp(0, originalAmbienceVolume, elapsedTime / timer);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        #endregion

        // Set final volume of ambience.
        ambientSource.volume = originalAmbienceVolume;
    }

    /// <summary>
    /// Call the coroutine which fades in the ambience track.
    /// </summary>
    private void PlayAmbience()
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(FadeInAmbience());
    }

    /// <summary>
    /// Play the default music clip. If there is no default clip, the looping clip will play.
    /// </summary>
    private void PlayMusic()
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(FadeAmbienceInAndThenStartMusic());
    }

    /// <summary>
    /// Play the original music track over the end credits.
    /// </summary>
    private void PlayEndCreditsMusic()
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(FadeAmbienceInAndThenStartMusicAtEndOfGame());
    }

    /// <summary>
    /// Stop all music and ambience.
    /// </summary>
    private void StopAmbience()
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(FadeAmbienceOut());
    }

    /// <summary>
    /// Stop the music, not the ambience.
    /// </summary>
    private void StopMusicOnly()
    {
        nonAmbientSource.Stop();
        titleSource.Stop();
    }

    private IEnumerator FadeAmbienceInAndThenStartMusic()
    {
        ambientSource.Stop();
        ambientSource.volume = originalAmbienceVolume;
        ambientSource.Play();

        yield return new WaitForSeconds(waitTime);

        ambientSource2.Play();
        nonAmbientSource.Play();
        titleSource.Play();

        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(WaitForSongToFinishThenSwitchSongs());
            StartCoroutine(FadeAmbienceOut());
        }
    }

    /// <summary>
    /// At the end of the game, stop all audio sources and play music.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeAmbienceInAndThenStartMusicAtEndOfGame()
    {
        // Stop absolutely every single audio source. We only want music now.
        foreach (AudioSource source in FindObjectsOfType<AudioSource>())
        {
            source.Stop();
        }

        yield return new WaitForSeconds(2);

        ambientSource.Stop();
        ambientSource.volume = originalAmbienceVolume;
        ambientSource.Play();

        yield return new WaitForSeconds(waitTime);

        ambientSource2.Play();
        nonAmbientSource.Play();
        titleSource.Play();

        StartCoroutine(WaitForSongToFinishThenSwitchSongs());
        StartCoroutine(FadeAmbienceOut());
    }

    private IEnumerator FadeAmbienceOut()
    {
        float elapsedTime = 0, timer = 3;
        while (elapsedTime < timer)
        {
            ambientSource.volume = Mathf.Lerp(originalAmbienceVolume, 0, elapsedTime / timer);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        ambientSource.volume = 0;
    }

    /// <summary>
    /// Wait until the music track has finished playing. Then, set the music's AudioSource to loop and play the looping track.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForSongToFinishThenSwitchSongs()
    {
        // Wait until the music track has finished playing.
        while (ambientSource2.isPlaying) yield return null;

        // Play the looping track.
        ambientSource2.Play();
        nonAmbientSource.Play();

        yield return null;
    }
}
