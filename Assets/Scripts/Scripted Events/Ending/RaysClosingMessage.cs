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
    private string[] plumLines, endingLines, endingLines2;

    [SerializeField, Tooltip("The voice telling the player what they must do.")]
    private AudioSource plumAudioSource;

    [SerializeField, Tooltip("The text speed, which must match the speed of the audio clip.")]
    private float textSpeedForPlumLine = 30;

    private void OnEnable()
    {
        EndingScreen.DoneWithLevels += DisplayRaysMessage;
    }
    private void OnDisable()
    {
        EndingScreen.DoneWithLevels -= DisplayRaysMessage;
    }

    /// <summary>
    /// Call coroutine that plays the audio and prints the text.
    /// </summary>
    private void DisplayRaysMessage()
    {
        StartCoroutine(TextSequence());
    }

    /// <summary>
    /// Play audio and display text.
    /// </summary>
    /// <returns></returns>
    private IEnumerator TextSequence()
    {
        // Clear text.
        textWriter.DisplayText("");

        // Adjust text speed to match audio clip.
        textWriter.ChangeTypingSpeed(textSpeedForPlumLine);

        // Play audio clip.
        plumAudioSource.Play();

        // print text while the audio clip is playing.
        textWriter.DisplayText(plumLines);
        while (plumAudioSource.isPlaying) yield return null;

        #region print Ray's sentimental message.  :(

        yield return new WaitForSeconds(2);
        textWriter.DisplayText(endingLines);
        yield return new WaitForSeconds(30);
        textWriter.DisplayText(endingLines2);

        #endregion

        if (FinishedMessage != null) FinishedMessage.Invoke();
    }
}
