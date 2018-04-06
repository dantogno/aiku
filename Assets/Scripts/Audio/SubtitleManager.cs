using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// SubtitleManager takes in and displays subtitles from VOAudio objects.
/// </summary>
public class SubtitleManager : MonoBehaviour
{
    [Tooltip("Base number of seconds each subtitle is shown")]
    [SerializeField] public float subtitlePadding;                         //Base time added to every line

    [Tooltip("Speed of subtitles")]
    [SerializeField, Range(0.1f, 1f)] public float TextSpeed;              //Speed in seconds per word of subtitles

    [Tooltip("Text component the Subtitle Manager prints to")]
    [SerializeField] private Text SubtitleTextComponent;                   //Text object to print to

	public static event Action<int> SubtitleFinished;                      //Event called when a subtitle is finished
    private static SubtitleManager instance;
    private Coroutine coroutine;


    // Use this for initialization
    void Start ()
    {
        SubtitleTextComponent.text = "";
    }

    private void Awake()
    {
        //This forces SubtitleManager to act as a singleton.
        //If an instance of SubtitleManager already exists, the new one will destroy itself.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// VOAudioTriggered event handler
    /// </summary>
    /// <param name="subtitle"></param>
    private void OnVOAudioTriggered(string subtitle)
    {
        HandleSubtitle(subtitle);
    }

    /// <summary>
    /// Splits the subtitle string into an array based on ~ and passes that array on to ShowSubtitles
    /// </summary>
    /// <param name="subtitle"></param>
    private void HandleSubtitle(string subtitle)
    {
        //PARSE SUBTITLE BY "~"
        string[] _subtitles = subtitle.Split('~');
        //SHOW SUBTITLE[]
        ShowSubtitles(_subtitles);
    }

    /// <summary>
    /// Starts the coroutine which displays subtitles and stops any previous VO coroutine.
    /// </summary>
    /// <param name="subtitles"></param>
    private void ShowSubtitles(string[] subtitles)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(ShowSubtitleAndWait(subtitles));
    }

    /// <summary>
    /// Coroutine which shows subtitles on screen at a standardized rate.
    /// </summary>
    /// <param name="subtitles"></param>
    /// <returns></returns>
    private IEnumerator ShowSubtitleAndWait(string[] subtitles)
    {
        foreach (string subtitle in subtitles)
        {
            //Counts the number of words per subtitle and uses this value to determine how long to display the subtitle.
            string[] subtitleWords = subtitle.Split(' ');
            SubtitleTextComponent.text = subtitle;
            yield return new WaitForSeconds((subtitlePadding + TextSpeed * subtitleWords.Length));
        }

        SubtitleTextComponent.text = "";

		if (SubtitleFinished != null)
			SubtitleFinished.Invoke (0);
    }

    /// <summary>
    /// Subcribes event handler on Enable.
    /// </summary>
    private void OnEnable()
    {
        // Event subscription
        VOAudio.VOAudioTriggered += OnVOAudioTriggered;
    }

    /// <summary>
    /// Unsubscribes event handler on Disable.
    /// </summary>
    private void OnDisable()
    {
        // Have to unsubscribe too!
        VOAudio.VOAudioTriggered -= OnVOAudioTriggered;
    }
}