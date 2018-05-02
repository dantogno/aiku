using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script can go on any object that can be interacted with.
/// This is a specialized version of ActivateObjects which
/// enables a number of objects. This script also plays a sound
/// and animates the door when it is interacted with.
/// </summary>
public class ActivateRingObjects : ActivateObjects
{
    [SerializeField]
    [Tooltip("The sound to play when this object is interacted with.")]
    private AudioSource soundToPlay;

    [SerializeField]
    [Tooltip("The animator that opens the hologram door.")]
    private Animator doorAnimator;

    /// <summary>
    /// Disable the connected light and GameObjects.
    /// </summary>
    protected override void Start()
    {
        // Set the animation state to closed.
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("isOpen", false);
        }
        // Defer other functionality to parent class.
        base.Start();
    }

    /// <summary>
    /// Enable the light, play a sound, and enable the connected GameObjects.
    /// </summary>
    /// <param name="agentInteracting"></param>
    public override void Interact(GameObject agentInteracting)
    {
        // Play the sound
        if (soundToPlay != null)
        {
            soundToPlay.Play();
        }
        // Set the animation state to open.
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("isOpen", true);
        }
        // Defer other interact functionality to parent class.
        base.Interact(agentInteracting);
    }
}
