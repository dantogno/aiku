using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script can go on any object that can be interacted with.
/// This is a specialized version of ActivateObjects which
/// enables a number of objects. This script also plays a sound
/// and turns on a light when it is interacted with.
/// </summary>
public class ActivateRingObjects : ActivateObjects
{
    [SerializeField]
    [Tooltip("The sound to play when this object is interacted with.")]
    private AudioSource soundToPlay;

    [SerializeField]
    [Tooltip("The light to turn on when this object is interacted with.")]
    private Light terminalLight;

    /// <summary>
    /// Disable the connected light and GameObjects.
    /// </summary>
    protected override void Start()
    {
        // Disable the light at the start.
        if (terminalLight != null)
        {
            terminalLight.enabled = false;
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
        // Turn on the light.
        if(terminalLight != null)
        {
            terminalLight.enabled = true;
        }
        // Defer othehr interact functionality to parent class.
        base.Interact(agentInteracting);
    }
}
