using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [SerializeField, Tooltip("The UI element of the text for the opening sequence.")]
    private Text uiText;

    [SerializeField, Tooltip("The time it takes for a single loop of the ellipses at the end.")]
    private float ellipsesTime = 2f;

    [SerializeField, Tooltip("Lines which are printed to the screen.")]
    private Lines[] startUpLines;

    // The ellipses animate while this varibale is true.
    private bool areEllipsesAnimating = false;

    private void Start()
    {
        StartCoroutine(StartUpTextSequence());
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
        string originalText = GetFullString(startUpLines[2].lines);
        StartCoroutine(AnimateEllipses(originalText));
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
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        areEllipsesAnimating = false;
    }

    /// <summary>
    /// Breaks down an array of strings into lines and returns it as a single string.
    /// </summary>
    /// <param name="lines"></param>
    /// <returns></returns>
    private string GetFullString(string[] lines)
    {
        string fullString = string.Empty;
        for (int i = 0; i < lines.Length; i++)
        {
            if (i != lines.Length - 1)
            {
                fullString += lines[i] + "\n";
            }
            else
            {
                fullString += lines[i];
            }
        }
        return fullString;
    }

    /// <summary>
    /// Animates three dots at the end of a given UI text.
    /// </summary>
    /// <param name="originalText"></param>
    /// <returns></returns>
    private IEnumerator AnimateEllipses(string originalText)
    {
        areEllipsesAnimating = true;
        float elapsedTime = 0;
        int numDots = 0;
        uiText.text = originalText;
        while (areEllipsesAnimating)
        {
            elapsedTime += Time.deltaTime;
            // Divided by 4 for each state of the dot string: {Empty, One, Two, Three}
            if (elapsedTime >= ellipsesTime / 4f)
            {
                // We want to animate three dots
                if (numDots < 3)
                {
                    uiText.text += ".";
                    numDots++;
                }
                else
                {
                    uiText.text = originalText;
                    numDots = 0;
                }
                elapsedTime = 0;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}