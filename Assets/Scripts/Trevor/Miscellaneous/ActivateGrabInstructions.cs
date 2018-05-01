using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Triggers the dialogue that instructs the player how to use the GMD
/// </summary>
public class ActivateGrabInstructions : MonoBehaviour
{
    private void OnEnable()
    {
        Scope.ScopedIn += ActivateInstructions;
    }

    private void OnDisable()
    {
        Scope.ScopedIn -= ActivateInstructions;
    }

    /// <summary>
    /// Activates the dialogue
    /// </summary>
    /// <param name="i"></param>
    private void ActivateInstructions(int i)
    {
        if(GetComponent<VOAudio>() != null)
            GetComponent<VOAudio>().TriggerVOAudio();
        this.enabled = false;
    }
}
