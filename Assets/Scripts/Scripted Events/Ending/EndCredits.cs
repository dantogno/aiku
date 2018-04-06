using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script handles the end credits sequence of our game.
/// The sequence is divided by "slides" - each slide shows a concentration (art, design, programming, etc.) and all the people in that concentration.
/// A slide stays onscreen for a short while, then fades out and is replaced by a new slide.
/// When there are no slides left, the game ends!
/// </summary>

public class EndCredits : MonoBehaviour
{
    public static event Action CreditsStarted;

    /// <summary>
    /// This is an inspector struct containing information for one "slide" of the end credits sequence (concentration and names of people in that concentration).
    /// </summary>
    [Serializable]
    private struct Credit
    {
        // Heading that will appear over the names in the end credits.
        public string Header;

        // Our happy family!
        public string[] Names;
    }

    [SerializeField, Tooltip("This panel is the backdrop over which the credits will roll.")]
    private Image endCreditsPanel;

    [SerializeField, Tooltip("This is the text object which will display the concentration of our team members.")]
    private Text headerText;

    [SerializeField, Tooltip("This is the text object which will display our team members' names.")]
    private Text nameText;

    [SerializeField, Tooltip("This is how much time we are giving the player to read each slide.")]
    private float slideWaitTime = 3;

    [SerializeField, Tooltip("This is how much time it takes for the text to fade in and out.")]
    private float textFadeTime = .5f;

    [SerializeField, Tooltip("Enter our game's credits in these fields!")]
    private Credit[] credits;

    private void OnEnable()
    {
        HubVFXManager.FinalVFXHasFinished += RollCredits;
    }
    private void OnDisable()
    {
        HubVFXManager.FinalVFXHasFinished -= RollCredits;
    }

    private void Start()
    {
        HideCreditsPanel();
    }

    /// <summary>
    /// Turn all the text and image objects invisible during gameplay.
    /// </summary>
    private void HideCreditsPanel()
    {
        headerText.text = "";
        nameText.text = "";

        endCreditsPanel.color = Color.clear;
        headerText.color = Color.clear;
        nameText.color = Color.clear;
    }

    /// <summary>
    /// Game is done!
    /// </summary>
    private void RollCredits()
    {
        StartCoroutine(CycleThroughCredits());
    }

    /// <summary>
    /// Call this coroutine from any other script to trigger the end credit sequence.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CycleThroughCredits()
    {
        if (CreditsStarted != null) CreditsStarted.Invoke();

        // This variable is re-used for each while loop as a timer.
        float elapsedTime = 0;

        endCreditsPanel.color = Color.black;

        yield return new WaitForSeconds(3);

        // This foreach loop cycles through the credits.
        foreach(Credit credit in credits)
        {
            SetCreditText(credit);

            #region Fade in text.

            elapsedTime = 0;
            while (elapsedTime < textFadeTime)
            {
                headerText.color = Color.Lerp(Color.clear, Color.white, elapsedTime / textFadeTime);
                nameText.color = Color.Lerp(Color.clear, Color.white, elapsedTime / textFadeTime);

                yield return new WaitForEndOfFrame();
                elapsedTime += Time.deltaTime;
            }

            headerText.color = Color.white;
            nameText.color = Color.white;

            #endregion

            // Allow some time for the player to read the names on the slide.
            yield return new WaitForSeconds(slideWaitTime);

            #region Fade out text.

            elapsedTime = 0;
            while (elapsedTime < textFadeTime)
            {
                headerText.color = Color.Lerp(Color.white, Color.clear, elapsedTime / textFadeTime);
                nameText.color = Color.Lerp(Color.white, Color.clear, elapsedTime / textFadeTime);

                yield return new WaitForEndOfFrame();
                elapsedTime += Time.deltaTime;
            }

            headerText.color = Color.clear;
            nameText.color = Color.clear;

            #endregion
        }

        yield return new WaitForSeconds(4);

        // After the credits are done, quit the game! Wow, this is a clean script. This is such a nice, clean script. Good job, me.
        Application.Quit();
    }

    /// <summary>
    /// Change the text on the screen.
    /// </summary>
    /// <param name="credit"></param>
    private void SetCreditText(Credit credit)
    {
        headerText.text = credit.Header;

        // Clear the previous names off the screen to make room for the new ones.
        nameText.text = "";

        // Add a name to the screen, followed by a line break. One name per line.
        for (int i = 0; i < credit.Names.Length; i++)
        {
            nameText.text += (credit.Names[i] + "\n");
        }
    }
}
