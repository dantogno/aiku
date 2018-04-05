using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Text which plays at beginning of game.
/// The script is applied to a dedicated GameObject.
/// </summary>

public class OpeningText : MonoBehaviour
{
    [Serializable]
    private struct Lines
    {
        public string[] lines;
    }

    [SerializeField, Tooltip("The text writer component that prints text to the screen.")]
    private TextWriter textWriter;

    [SerializeField, Tooltip("Lines which are printed to the screen.")]
    private Lines[] startUpLines;

    // The player can only press skip once, to prevent button-mashing.
    private bool canSkip = true;

    private void Start()
    {
        StartCoroutine(StartUpTextSequence());
    }

    private void Update()
    {
        // Player can skip intro.
        if (Input.GetButtonDown("Interact") && canSkip)
        {
            StartCoroutine(LoadNextScene());
        }
    }

    /// <summary>
    /// Print text to screen.
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartUpTextSequence()
    {
        yield return new WaitForSeconds(2);

        textWriter.DisplayText(startUpLines[0].lines);
        yield return StartCoroutine(ClearTextAfterWait(10));
        textWriter.DisplayText(startUpLines[1].lines);
        yield return StartCoroutine(ClearTextAfterWait(12));
        textWriter.DisplayText(startUpLines[2].lines);
        StartCoroutine(LoadNextScene());
    }

    /// <summary>
    /// Get rid of all text on the screen.
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    private IEnumerator ClearTextAfterWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        textWriter.DisplayText("");
    }

    /// <summary>
    /// Start the game!
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadNextScene()
    {
        canSkip = false;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
