using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls the message that plays on the player's screen after the levels have been completed.
/// The script is applied to the computer text canvas GameObject, which is a child of the player's main camera.
/// </summary>

public class RaysClosingMessage : MonoBehaviour
{
    public static event Action FinishedMessage;

    [SerializeField, Tooltip("The text writer component that prints text to the screen.")]
    private TextWriter textWriter;

    [SerializeField, Tooltip("text lines to print to the screen.")]
    private string[] plumLines, endingLines, endingLines2, endingLines3, foundAllPowerLines;

    [SerializeField, Tooltip("The voice telling the player what they must do.")]
    private AudioSource plumAudioSource, rayAudioSource;

    [SerializeField, Tooltip("The text speed, which must match the speed of the audio clip.")]
    private float textSpeedForPlumLine = 30;

    [SerializeField, Tooltip("The brackets which appear over interactable objects.")]
    private GameObject scanningCam;

    private void OnEnable()
    {
        Cryochamber.AddedPowerToCryochamberForFirstTime += DisplayPlumMessage;
        EndingScreen.AllocatedAllShipboardPowerToCryochambers += DisplayRaysMessage;
        EndingScreen.FoundAllPower += DisplayFoundAllPowerMessage;
    }
    private void OnDisable()
    {
        Cryochamber.AddedPowerToCryochamberForFirstTime -= DisplayPlumMessage;
        EndingScreen.AllocatedAllShipboardPowerToCryochambers -= DisplayRaysMessage;
        EndingScreen.FoundAllPower -= DisplayFoundAllPowerMessage;
    }

    /// <summary>
    /// When the player transfers power to a cryochamber for the first time, Plum tells them to enter the simulations.
    /// </summary>
    private void DisplayPlumMessage()
    {
        // Clear text.
        textWriter.DisplayText("");

        // Adjust text speed to match audio clip.
        textWriter.ChangeTypingSpeed(textSpeedForPlumLine);

        // Play audio clip.
        plumAudioSource.Play();

        // print text while the audio clip is playing.
        textWriter.DisplayText(plumLines);
    }

    /// <summary>
    /// Call coroutine that plays the audio and prints the text.
    /// </summary>
    private void DisplayRaysMessage()
    {
        StartCoroutine(RayTextSequence());
    }

    /// <summary>
    /// Show this message when the player finds all the power.
    /// </summary>
    private void DisplayFoundAllPowerMessage()
    {
        textWriter.DisplayText(foundAllPowerLines);
    }

    /// <summary>
    /// If the ending happens, go silent.
    /// </summary>
    private void CutMessageShort()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Play audio and display text.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RayTextSequence()
    {
        // Disable interaction until Ray has left the player a puddle of emotions.
        InteractWithSelectedObject interactionScript = Camera.main.GetComponent<InteractWithSelectedObject>();

        if (interactionScript != null && scanningCam != null)
        {
            interactionScript.enabled = false;
            scanningCam.SetActive(false);
        }

        // Clear text.
        textWriter.DisplayText("");
        
        // Play audio clip.
        rayAudioSource.Play();

        #region print Ray's sentimental message.  :(

        // Adjust text speed to match audio clip (we are hardcoding this because Becca's talking speed changes from line to line).
        textWriter.ChangeTypingSpeed(17);

        textWriter.DisplayText(endingLines);
        yield return new WaitForSeconds(18);

        // Adjust text speed to match audio clip (we are hardcoding this because Becca's talking speed changes from line to line).
        textWriter.ChangeTypingSpeed(18);

        textWriter.DisplayText(endingLines2);
        yield return new WaitForSeconds(17);

        // Adjust text speed to match audio clip (we are hardcoding this because Becca's talking speed changes from line to line).
        textWriter.ChangeTypingSpeed(20);

        textWriter.DisplayText(endingLines3);

        #endregion

        while (rayAudioSource.isPlaying) yield return null;

        // Enable interaction, so that the player can kill themself.
        if (interactionScript != null)
        {
            interactionScript.enabled = true;
            scanningCam.SetActive(true);
        }

        if (FinishedMessage != null) FinishedMessage.Invoke();
    }
}
