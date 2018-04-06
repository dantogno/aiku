using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for the SOS button on the bridge.
/// The script can be applied to the SOS button on the bridge.  :)
/// </summary>

public class SOSButton : MonoBehaviour, IInteractable
{
    public static event Action PressedButton;

    [SerializeField, Tooltip("When the SOS button is pressed, this task is completed.")]
    private Task sosTask;

    [SerializeField, Tooltip("The canvas to turn off after pressing the button.")]
    private GameObject sosCanvas;

    public void Interact(GameObject interactingAgent)
    {
        PlaySound();
        TurnOffEmission();
        DisableSOSCanvas();
        DisableCollision();

        if (PressedButton != null) PressedButton.Invoke();
    }

    /// <summary>
    /// The SOS button emits a recording of Norma sending out a message.
    /// </summary>
    private void PlaySound()
    {
        AudioSource myAudioSource = GetComponent<AudioSource>();
        myAudioSource.Play();
    }

    /// <summary>
    /// We want the button's light to turn off, so that the player knows not to interact with it anymore.
    /// </summary>
    private void TurnOffEmission()
    {
        GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
    }

    /// <summary>
    /// The canvas pointing to the SOS button is no longer necessary.
    /// </summary>
    private void DisableSOSCanvas()
    {
        sosCanvas.SetActive(false);
    }

    /// <summary>
    /// Disable the collision, and therefore prevent future interaction.
    /// </summary>
    private void DisableCollision()
    {
        Collider myCollider = GetComponent<Collider>();
        myCollider.enabled = false;
    }
}
