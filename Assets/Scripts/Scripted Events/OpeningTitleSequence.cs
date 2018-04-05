using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// This script handles the game's title sequence.
/// It is applied to a dedicated canvas in the hub scene.
/// </summary>

public class OpeningTitleSequence : MonoBehaviour
{
    [SerializeField, Tooltip("The image object containing the game's title text.")]
    private Image titleImage;

    [SerializeField, Tooltip("This allows the title text to fade smoothly - more smoothly than a lerp.")]
    private AnimationCurve TitleImageFadeCurve;

    [SerializeField, Tooltip("This is the color of the title text.")]
    private Color titleImageColor;

    [SerializeField, Tooltip("How long it takes the title text to fade in.")]
    private float titleImageFadeInTime = 3;

    [SerializeField, Tooltip("How long the title text is onscreen for.")]
    private float titleImageWaitTime = 1.5f;

    [SerializeField, Tooltip("How long it takes the title text to fade out.")]
    private float titleImageFadeOutTime = 1.5f;

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
        HideTitleImage();
    }

    /// <summary>
    /// All the text objects should start clear, so that they only appear when needed.
    /// </summary>
    private void HideTitleImage()
    {
        titleImage.color = Color.clear;
    }

    /// <summary>
    /// The title text is displayed when the generator shuts down.
    /// </summary>
    private void DisplayTitleText()
    {
        StartCoroutine(TitleSequence());
    }

    /// <summary>
    /// Fade the title of the game in, and then out.
    /// </summary>
    /// <returns></returns>
    private IEnumerator TitleSequence()
    {
        yield return new WaitForSeconds(titleImageWaitTime);

        yield return StartCoroutine(FadeImage(titleImage, titleImageColor, titleImageFadeInTime, true));

        yield return new WaitForSeconds(titleImageWaitTime);

        yield return StartCoroutine(FadeImage(titleImage, titleImageColor, titleImageFadeInTime, false));
    }

    /// <summary>
    /// Fade image in or out.
    /// </summary>
    /// <param name="imageToFade"></param>
    /// <param name="imageColor"></param>
    /// <param name="fadeTime"></param>
    /// <param name="fadeIn"></param>
    /// <returns></returns>
    private IEnumerator FadeImage(Image imageToFade, Color imageColor, float fadeTime, bool fadeIn)
    {
        // Fades look weird if their non-alpha channels do not match their counterpart's color.
        Color clear = imageColor == titleImageColor ?
            new Color(titleImageColor.r, titleImageColor.g, titleImageColor.b, 0) : new Color(1, 1, 1, 0);

        // Figure out which colors we want to lerp between, based on whether we are fading the text in or out.
        Color originalColor = fadeIn ? clear : imageColor,
            targetColor = fadeIn ? imageColor : clear;

        #region Lerp between the two colors.

        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            imageToFade.color = Color.Lerp(originalColor, targetColor, TitleImageFadeCurve.Evaluate(elapsedTime / fadeTime));

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        #endregion

        // Set the text color to the target color.
        imageToFade.color = targetColor;
    }
}
