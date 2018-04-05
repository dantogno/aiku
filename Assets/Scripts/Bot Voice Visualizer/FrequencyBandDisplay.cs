using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;


//TODO: Remove all "FormerlySerializedAs" tags once scene has been successfully saved

public class FrequencyBandDisplay : MonoBehaviour
{
    private const int numBands = 8;

    [SerializeField]
    [FormerlySerializedAs("_audioAnalysis")]
    [Tooltip("AudioAnalysis component to show data from")]
    AudioAnalysis audioAnalysis;

    [SerializeField]
    [FormerlySerializedAs("_band")]
    [Tooltip("Frequency band to show")]
    [Range(0, 7)]
    private int band;

    [SerializeField]
    [FormerlySerializedAs("_tinted")]
    [Tooltip("Use AudioAnalysis Bass, Mid, and High colors instead of image color")]
    private bool tinted = false;

    private Image image;

    // Use this for initialization
    private void Start()
    {
        image = GetComponent<Image>();

        if (tinted)
        {
            if (band <= (float)numBands / 2)
            {
                image.color = Color.Lerp(audioAnalysis.BassColor, audioAnalysis.MidColor, (float)band / ((float)numBands / 2));
            }
            else
            {
                image.color = Color.Lerp(audioAnalysis.MidColor, audioAnalysis.HighColor, ((float)band - ((float)numBands / 2)) / ((float)numBands / 2));
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //4 is kind of arbitrary, I found that it looked nice at the volume most VO is at
        image.fillAmount = audioAnalysis.AudioBandBuffer[band] * 4;
    }
}
