using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Types characters one at a time
/// </summary>
public class TextWriter : MonoBehaviour {

    [SerializeField]
    [Tooltip("Text UI element where we should type")]
    private Text uiText;

    [SerializeField]
    [Tooltip("Characters per second")]
    private float typingSpeed;
    
    [SerializeField]
    [Tooltip("Wait time before beginnning a new line")]
    private float newLineDelay;

    [SerializeField]
    [Tooltip("Blocks are typed out one at a time and each block replaces the previous")]
    private string[] textBlock1, textBlock2, textBlock3;

    [SerializeField]
    [Tooltip("The component associated with the noise the text makes as it prints characters to the screen.")]
    private AudioSource myAudioSource;  // DW

    private string[][] textStrings;

    private Coroutine type, blink;

	// Use this for initialization
	private void Start () {
        //this is my cheap way to get an array of arrays to show up in the inspector
        string[][] assembledBlocks = { textBlock1, textBlock2, textBlock3 };

        textStrings = assembledBlocks;
        if (myAudioSource == null) myAudioSource = GetComponent<AudioSource>();    // DW

        float pitch = myAudioSource.pitch;
        myAudioSource.pitch = Random.Range(pitch - .05f, pitch + .05f);   // DW
    }

    /// <summary>
    /// Called by animations to show text as part of animation cycle
    /// </summary>
    public void TypeText()
    {
        StopCoroutine(Go());
        StartCoroutine(Go());
    }

    /// <summary>
    /// Erases all other text on the screen, then types out the given line
    /// </summary>
    /// <param name="text">The text to type</param>
    /// <returns></returns>
    public void DisplayText(string text)
    {
        if(type != null)
            StopCoroutine(type);

        type = StartCoroutine(TypeTextLineByLine(text));
    }

    /// <summary>
    /// DisplayText implementation that accepts an array
    /// </summary>
    /// <param name="text">Array of strings. Each string is typed on its own line</param>
    /// <returns></returns>
    public void DisplayText(string[] text)
    {
        if(type != null)
            StopCoroutine(type);

        type = StartCoroutine(TypeTextLineByLine(text));
    }

    public void ChangeTypingSpeed(float newSpeed)
    {
        typingSpeed = newSpeed;
    }

    /// <summary>
    /// Erases all other text on the screen, then types the given line
    /// </summary>
    /// <param name="text">The text to type</param>
    /// <returns></returns>
    private IEnumerator TypeTextLineByLine(string text)
    {
        if(blink != null)
            StopCoroutine(blink);

        float startTime = Time.time;
        int currentPosition = 0;
        while (currentPosition < text.Length)
        {
            float timeElapsed = Time.time - startTime;
            currentPosition = Mathf.Clamp(Mathf.FloorToInt(timeElapsed * typingSpeed), 0, text.Length);

            uiText.text = text.Substring(0, currentPosition);  // DW: deleted cursor
            yield return null;
        }

        uiText.text = text;
    }

    /// <summary>
    /// typeText implementation that accepts an array
    /// </summary>
    /// <param name="text">Array of strings. Each string is typed on its own line</param>
    /// <returns></returns>
    private IEnumerator TypeTextLineByLine(string[] text)
    {
        if (blink != null)
            StopCoroutine(blink);

        float startTime = Time.time;
        int currentPosition = 0;
        int currentLine = 0;
        string previousLines = "";
        while (currentLine < text.Length)
        {
            while (currentPosition < text[currentLine].Length)
            {
                if (!myAudioSource.isPlaying) myAudioSource.Play(); // DW

                float timeElapsed = Time.time - startTime;
                currentPosition = Mathf.Clamp(Mathf.FloorToInt(timeElapsed * typingSpeed), 0, text[currentLine].Length);

                uiText.text = previousLines + text[currentLine].Substring(0, currentPosition);  // DW: deleted cursor
                yield return null;
            }

            previousLines += text[currentLine];
            startTime = Time.time;
            currentPosition = 0;
            currentLine++;

            if(currentLine < text.Length)
            {
                previousLines += "\n";
            }


            yield return new WaitForSeconds(newLineDelay);
        }

        uiText.text = previousLines;
    }

    /// <summary>
    /// Flashes the | character at the end of the current line to simulate a text editor's cursor
    /// </summary>
    /// <param name="blinkTime"></param>
    /// <returns></returns>
    private IEnumerator BlinkCursor(float blinkTime)
    {
        bool cursor = false;
        string text = uiText.text;
        while (true)
        {
            cursor = !cursor;

            if (cursor)
            {
                uiText.text = text + " |";
            }
            else
            {
                uiText.text = text;
            }

            yield return new WaitForSeconds(blinkTime);
        }
    }

    /// <summary>
    /// Types out all three text blocks one after another
    /// </summary>
    /// <returns></returns>
    private IEnumerator Go()
    {

        for(int i = 0; i < textStrings.Length; i++)
        {
            if (textStrings[i].Length == 0)
                break;

            DisplayText(textStrings[i]);

            yield return new WaitForSeconds(7);
        }
    }
}
