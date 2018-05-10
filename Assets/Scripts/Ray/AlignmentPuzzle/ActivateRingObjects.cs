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

    [SerializeField]
    [Tooltip("The green hovering arrow to deactivate.")]
    private GameObject arrow;

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
    /// Set this to the correct animation state when it is enabled.
    /// </summary>
    private void OnEnable()
    {
        PlayAnimation();
    }

    /// <summary>
    /// Enable the light, play a sound, and enable the connected GameObjects.
    /// </summary>
    /// <param name="agentInteracting"></param>
    public override void Interact(GameObject agentInteracting)
    {
        // Defer basic interact functionality to parent class.
        base.Interact(agentInteracting);

        // Play the sound
        if (soundToPlay != null)
        {
            soundToPlay.Play();
        }
        // Play the correct animation.
        PlayAnimation();

        if (arrow != null)
        {
            arrow.SetActive(false);
        }
    }

    /// <summary>
    /// Plays an animation based on the current state.
    /// </summary>
    private void PlayAnimation()
    {
        if (!active)
        {
            // Set the animation state to closed.
            if (doorAnimator != null)
            {
                doorAnimator.SetBool("isOpen", false);
            }
        }
    }
}
