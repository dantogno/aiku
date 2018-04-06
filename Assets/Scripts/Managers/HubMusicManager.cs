using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls when the music plays in the hub level. It is distinct from the HubAudioManager class, which manages other audio functionality.
/// Apply this script to a dedicated GameObject alongside any other managers, so that it is easy to find.
/// </summary>

[RequireComponent(typeof(AudioSource))]
public class HubMusicManager : MonoBehaviour
{
    [SerializeField, Tooltip("This is the music track which will loop constantly as the player plays through the power transfer puzzles.")]
    private AudioClip loopingClip;

    // The AudioSource component attached to this GameObject.
    private AudioSource musicSource;

    // The AudioSource's default audio clip.
    private AudioClip originalClip;

    private void OnEnable()
    {
        EngineSequenceManager.OnShutdown += PlayMusic;
        EndingScreen.DoneWithLevels += StopMusic;
        EndCredits.CreditsStarted += PlayEndCreditsMusic;
    }
    private void OnDisable()
    {
        EngineSequenceManager.OnShutdown -= PlayMusic;
        EndingScreen.DoneWithLevels -= StopMusic;
        EndCredits.CreditsStarted -= PlayEndCreditsMusic;
    }

    /// <summary>
    /// Play the default music clip. If there is no default clip, the looping clip will play.
    /// </summary>
    private void PlayMusic()
    {
        musicSource = GetComponent<AudioSource>();

        if (musicSource.clip == null) musicSource.clip = loopingClip;
        
        originalClip = musicSource.clip;
        musicSource.Play();

        StartCoroutine(WaitForSongToFinishThenSwitchSongs());
    }

    /// <summary>
    /// Play the original music track over the end credits.
    /// </summary>
    private void PlayEndCreditsMusic()
    {
        musicSource.clip = originalClip;
        musicSource.Play();
    }

    /// <summary>
    /// Stop the music.
    /// </summary>
    private void StopMusic()
    {
        musicSource.Stop();
    }

    /// <summary>
    /// Wait until the music track has finished playing. Then, set the music's AudioSource to loop and play the looping track.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForSongToFinishThenSwitchSongs()
    {
        // Wait until the music track has finished playing.
        while (musicSource.isPlaying) yield return null;

        // Set the AudioSource clip to the correct track, if it is not set already.
        musicSource.clip = loopingClip;

        // Set the music's AudioSource to loop.
        musicSource.loop = true;

        // Play the looping track.
        musicSource.Play();
    }
}
