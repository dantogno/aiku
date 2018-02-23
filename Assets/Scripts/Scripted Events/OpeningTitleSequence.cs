using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// This script handles the game's title sequence.
/// It is applied to a prefabbed Canvas in the opening scene (not the Hub scene).
/// </summary>

public class OpeningTitleSequence : MonoBehaviour
{
    [SerializeField, Tooltip("The opening text is shown right when the game starts, introducing the game.")]
    private Text openingText;

    [SerializeField, Tooltip("The text object containing the first half of the game's title.")]
    private Text coldText;

    [SerializeField, Tooltip("The text object containing the second half of the game's title.")]
    private Text sleepText;

    [SerializeField, Tooltip("This allows thye title text to fade smoothly - more smoothly than a lerp.")]
    private AnimationCurve TitleTextFadeCurve;

    [SerializeField, Tooltip("This is the color of the title text.")]
    private Color titleTextColor;

    [SerializeField, Tooltip("How long it takes the opening text to fade in.")]
    private float openingTextFadeTime = .5f;

    [SerializeField, Tooltip("How long the opening text is onscreen for.")]
    private float openingTextWaitTime = 2.5f;

    [SerializeField, Tooltip("How long it takes the title text to fade in.")]
    private float titleTextFadeInTime = 2.5f;

    [SerializeField, Tooltip("How long it takes the title text to fade out.")]
    private float titleTextFadeOutTime = 1.5f;

    [SerializeField, Tooltip("The time between when the words 'Cold' and 'Sleep' (the game's title) fade in and out.")]
    private float titleTextStaggerTime = 1;

    [SerializeField, Tooltip("How long the title text is onscreen for.")]
    private float titleTextWaitTime = 1;

    private void OnEnable()
    {
        EngineSequenceManager.OnShutdown += DisplayTitleText;
    }
    private void OnDisable()
    {
        EngineSequenceManager.OnShutdown -= DisplayTitleText;
    }

    private void Start()
    {
        HideText();
        StartCoroutine(OpeningSequence());
    }

    /// <summary>
    /// All the text objects should start clear, so that they only appear when needed.
    /// </summary>
    private void HideText()
    {
        openingText.color = Color.clear;
        coldText.color = Color.clear;
        sleepText.color = Color.clear;
    }

    /// <summary>
    /// The title text is displayed when the generator shuts down.
    /// </summary>
    private void DisplayTitleText()
    {
        StartCoroutine(TitleSequence());
    }

    /// <summary>
    /// This short sequence presents the game to the player right at the very beginning.
    /// After the text fades in and out, the first scene of the game is loaded.
    /// </summary>
    /// <returns></returns>
    private IEnumerator OpeningSequence()
    {
        #region Fade text in and out.

        yield return new WaitForSeconds(openingTextFadeTime);

        yield return StartCoroutine(FadeText(openingText, Color.white, openingTextFadeTime, true));

        yield return new WaitForSeconds(openingTextWaitTime);

        yield return StartCoroutine(FadeText(openingText, Color.white, openingTextFadeTime, false));

        yield return new WaitForSeconds(openingTextFadeTime);

        #endregion

        // Load the game.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// We stagger the fade-in and fade-out of the two words in the game's title, because it looks cooler.
    /// </summary>
    /// <returns></returns>
    private IEnumerator TitleSequence()
    {
        yield return new WaitForSeconds(titleTextStaggerTime);

        // We are not using yield return StartCoroutine for this line, because we want the fades to overlap.
        StartCoroutine(FadeText(coldText, titleTextColor, titleTextFadeInTime, true));
        
        yield return new WaitForSeconds(titleTextStaggerTime);

        yield return StartCoroutine(FadeText(sleepText, titleTextColor, titleTextFadeInTime, true));

        yield return new WaitForSeconds(titleTextWaitTime);

        // We are not using yield return StartCoroutine for this line, because we want the fades to overlap.
        StartCoroutine(FadeText(coldText, titleTextColor, titleTextFadeInTime, false));

        yield return new WaitForSeconds(titleTextStaggerTime);

        // We are not using yield return StartCoroutine for this line, because we want the fades to overlap.
        StartCoroutine(FadeText(sleepText, titleTextColor, titleTextFadeInTime, false));
    }

    /// <summary>
    /// Fade text in or out.
    /// </summary>
    /// <param name="textToFade"></param>
    /// <param name="textColor"></param>
    /// <param name="fadeTime"></param>
    /// <param name="fadeIn"></param>
    /// <returns></returns>
    private IEnumerator FadeText(Text textToFade, Color textColor, float fadeTime, bool fadeIn)
    {
        // Fades look weird if their non-alpha channels do not match their counterpart's color.
        Color clear = textColor == titleTextColor ?
            new Color(titleTextColor.r, titleTextColor.g, titleTextColor.b, 0) : new Color(1, 1, 1, 0);
        
        // Figure out which colors we want to lerp between, based on whether we are fading the text in or out.
        Color originalColor = fadeIn ? clear : textColor,
            targetColor = fadeIn ? textColor : clear;

        #region Lerp between the two colors.

        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            textToFade.color = Color.Lerp(originalColor, targetColor, TitleTextFadeCurve.Evaluate(elapsedTime / fadeTime));

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        #endregion

        // Set the text color to the target color.
        textToFade.color = targetColor;
    }
}
