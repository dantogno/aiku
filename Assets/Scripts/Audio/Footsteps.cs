using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plays footstep sound while the player is walking. Should be attached to the player game object
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Footsteps : MonoBehaviour
{
    [Tooltip("Put your footstep sounds here.")]
    [SerializeField] private AudioClip[] footstepClips;

    [Tooltip("Desired footstep clip")]
    [SerializeField] private AudioClip desiredFootstepClip;

    [Tooltip("The general pitch of the footsteps. This will be randomized to a degree with each step.")]
    [Range(0, 1)]
    [SerializeField] private float footstepPitch;

    private CustomRigidbodyFPSController playerControllerScript;
    private AudioSource footstepsAudio;

	// Use this for initialization
	private void Start ()
    {
        playerControllerScript = GetComponent<CustomRigidbodyFPSController>();
        footstepsAudio = GetComponent<AudioSource>();
	}
	
	/// <summary>
    /// If the player is on the ground and moving, play footsteps.
    /// </summary>
	private void Update ()
    {
        if (playerControllerScript.Grounded == true &&
            playerControllerScript.Velocity.magnitude > 2f &&
            footstepsAudio.isPlaying == false && 
            desiredFootstepClip != null)
        {
            footstepsAudio.clip = desiredFootstepClip;
            footstepsAudio.pitch = Random.Range(footstepPitch - 0.1f, footstepPitch + 0.1f);
            footstepsAudio.Play();
        }
	}

    /// <summary>
    /// Change the footstep clip to specified clip.
    /// </summary>
    /// <param name="footstep"></param>
    public void ChangeDesiredFootstep(string footstep)
    {
        for (int i = 0; i < footstepClips.Length; i++)
        {
            if (footstepClips[i].name == footstep)
            {
                desiredFootstepClip = footstepClips[i];
            }
        }
    }
}
