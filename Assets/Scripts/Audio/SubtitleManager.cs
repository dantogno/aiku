using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleManager : MonoBehaviour
{
    [Tooltip("Base number of seconds each subtitle is shown")]
    [SerializeField] public float subtitlePadding;                         //Base time added to every line

    [Tooltip("Speed of subtitles")]
    [SerializeField, Range(0.1f, 1f)] public float TextSpeed;       //Speed in seconds per word of subtitles

    [Tooltip("Text component the Subtitle Manager prints to")]
    [SerializeField] private Text SubtitleTextComponent;                    //Text object to print to

	public static event Action<int> SubtitleFinished;
    private static SubtitleManager instance;
    private Coroutine coroutine;


    // Use this for initialization
    void Start ()
    {
        SubtitleTextComponent.text = "";
    }

    private void Awake()
    {
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

    private void OnVOAudioTriggered(string subtitle)
    {
        HandleSubtitle(subtitle);
    }

    private void HandleSubtitle(string subtitle)
    {
        //PARSE SUBTITLE BY "~"
        string[] _subtitles = subtitle.Split('~');
        //SHOW SUBTITLE[]
        ShowSubtitles(_subtitles);
    }

    private void ShowSubtitles(string[] subtitles)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(ShowSubtitleAndWait(subtitles));
    }

    private IEnumerator ShowSubtitleAndWait(string[] subtitles)
    {
        foreach (string subtitle in subtitles)
        {
            string[] subtitleWords = subtitle.Split(' ');
            SubtitleTextComponent.text = subtitle;
            yield return new WaitForSeconds((subtitlePadding + TextSpeed * subtitleWords.Length));
            //Debug.Log(subtitle);
        }

        SubtitleTextComponent.text = "";

		if (SubtitleFinished != null)
			SubtitleFinished.Invoke (0);
    }

    private void OnEnable()
    {
        // Event subscription
        VOAudio.VOAudioTriggered += OnVOAudioTriggered;
    }

    private void OnDisable()
    {
        // Have to unsubscribe too!
        VOAudio.VOAudioTriggered -= OnVOAudioTriggered;
    }
}