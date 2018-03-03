
using UnityEngine;

/// <summary>
///Our claw's audio is played from here,
/// </summary>

public class ClawAudioPlayer : MonoBehaviour {
   
    #region EditorFields
    [Tooltip("The audio clip that will play when the claw is moving horizontally")]
    [SerializeField]
    private AudioClip aClipMove;

    [Tooltip("The audio clip that will play when the claw is moving vertically")]
    [SerializeField]
    private AudioClip aClipDrop;

    [Tooltip("The audio clip that will play when the claw is opening / closing")]
    [SerializeField]
    private AudioClip aClipClose;
    #endregion

    #region Private fields
    //This will be the audio source for aClipVertical
    private AudioSource audioSource;  
    #endregion

    public void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        audioSource.clip = aClipMove;
    }

    // Plays the sound for our claw whenever it moves across the X or Z axis. 
    public void PlayHorizontalMoveAudio()
    {
        if (!audioSource.isPlaying)  // Inplace to ensure that our audio keeps looping and dosen't restart each frame input is held down.
        {
            audioSource.clip = aClipMove;
            audioSource.Play();
        }
    }

    // Plays the sound for our claw whenever it moves on the Y axis. 
    public void PlayClawDropAudio() 
    {
        audioSource.clip = aClipDrop;
        if (!audioSource.isPlaying && audioSource.clip == aClipDrop)
        {
            audioSource.Play();
        }
    }
   
    // Plays the sound for when our claw pauses to pick up the box.
    public void PlayClawAttachAudio() 
    {
        audioSource.clip = aClipClose;
        if (!audioSource.isPlaying && audioSource.clip == aClipClose)
        {
            audioSource.Play(); 
        }
    }

    // A stop for our audio. 
    public void StopAudio()
    {
        audioSource.Stop();
    }



}
