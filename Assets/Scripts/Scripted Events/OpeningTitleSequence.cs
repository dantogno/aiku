using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// I'm sorry, I don't want to comment this one. I'll do it on Friday.
/// </summary>

public class OpeningTitleSequence : MonoBehaviour
{
    [SerializeField, Tooltip("")]
    private Image openingPanel;

    [SerializeField, Tooltip("")]
    private Text openingText;

    [SerializeField, Tooltip("")]
    private Text coldText;

    [SerializeField, Tooltip("")]
    private Text sleepText;

    [SerializeField, Tooltip("")]
    private AnimationCurve TitleTextFadeCurve;

    [SerializeField, Tooltip("")]
    private float openingTextFadeTime = .5f;

    [SerializeField, Tooltip("")]
    private float openingTextWaitTime = 2.5f;

    [SerializeField, Tooltip("")]
    private float titleTextFadeInTime = 2.5f;

    [SerializeField, Tooltip("")]
    private float titleTextFadeOutTime = 1.5f;

    [SerializeField, Tooltip("")]
    private float titleTextStaggerTime = 1;

    [SerializeField, Tooltip("")]
    private float titleTextWaitTime = 2;

    private Color blue, clearBlue, clearWhite;

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
        StartCoroutine(FadeOpeningTextInAndOut());
    }

    private void HideText()
    {
        blue = coldText.color;
        clearBlue = new Color(blue.r, blue.g, blue.b, 0);
        clearWhite = new Color(1, 1, 1, 0);

        openingText.color = clearWhite;
        coldText.color = clearBlue;
        sleepText.color = clearBlue;
    }

    private void DisplayTitleText()
    {
        StartCoroutine(FadeTitleTextInAndThenOut());
    }

    private IEnumerator FadeOpeningTextInAndOut()
    {
        yield return new WaitForSeconds(openingTextFadeTime);

        float elapsedTime = 0;
        while (elapsedTime < openingTextFadeTime)
        {
            openingText.color = Color.Lerp(clearWhite, Color.white, elapsedTime / openingTextFadeTime);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        openingText.color = Color.white;

        yield return new WaitForSeconds(openingTextWaitTime);

        elapsedTime = 0;
        while (elapsedTime < openingTextFadeTime)
        {
            openingText.color = Color.Lerp(Color.white, clearWhite, elapsedTime / openingTextFadeTime);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        openingText.color = clearWhite;

        yield return new WaitForSeconds(openingTextFadeTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        openingPanel.color = Color.clear;
    }

    private IEnumerator FadeTitleTextInAndThenOut()
    {
        StartCoroutine(FadeText(coldText, true));
        
        yield return new WaitForSeconds(titleTextStaggerTime);
        
        StartCoroutine(FadeText(sleepText, true));

        yield return new WaitForSeconds(titleTextWaitTime);

        StartCoroutine(FadeText(coldText, false));

        yield return new WaitForSeconds(titleTextStaggerTime);

        StartCoroutine(FadeText(sleepText, false));
    }

    private IEnumerator FadeText(Text textToFade, bool fadeIn)
    {
        Color originalColor = fadeIn ? clearBlue : blue,
            targetColor = fadeIn ? blue : clearBlue;

        float elapsedTime = 0;
        while (elapsedTime < titleTextFadeInTime)
        {
            textToFade.color = Color.Lerp(originalColor, targetColor, TitleTextFadeCurve.Evaluate(elapsedTime / titleTextFadeInTime));

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        textToFade.color = targetColor;
    }
}
