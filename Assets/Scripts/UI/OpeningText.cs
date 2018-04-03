using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class controls the bootup text that plays over the game's opening scene.
/// </summary>

public class OpeningText : MonoBehaviour
{
    #region Text writer component and lines to feed into text writer.

    [Serializable]
    private struct Lines { public string[] lines; }

    [SerializeField]
    private TextWriter textWriter;

    [SerializeField]
    private Lines[] startUpLines;

    #endregion

    // Controls whether the player can click the mouse to skip over the opening sequence.
    private bool canSkip = true;

    private void Start()
    {
        StartCoroutine(StartUpTextSequence());
    }

    private void Update()
    {
        // Load next scene if the player presses the skip button.
        if (Input.GetButtonDown("Interact") && canSkip)
        {
            StartCoroutine(LoadNextScene());
        }
    }

    /// <summary>
    /// Opening text sequence of the game.
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartUpTextSequence()
    {
        yield return new WaitForSeconds(2);

        #region Display startup text lines sequentially.

        textWriter.DisplayText(startUpLines[0].lines);
        yield return StartCoroutine(ClearTextAfterWait(10));
        textWriter.DisplayText(startUpLines[1].lines);
        yield return StartCoroutine(ClearTextAfterWait(12));
        textWriter.DisplayText(startUpLines[2].lines);

        #endregion

        // When finished printing lines, load next scene.
        StartCoroutine(LoadNextScene());
    }

    /// <summary>
    /// Clear text after x seconds.
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    private IEnumerator ClearTextAfterWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        textWriter.DisplayText("");
    }

    /// <summary>
    /// Load the next scene in the build settings.
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
