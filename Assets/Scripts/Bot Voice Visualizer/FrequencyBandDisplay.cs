using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;


//TODO: Remove all "FormerlySerializedAs" tags once scene has been successfully saved

/// <summary>
/// Displays one of the frequency bands from an AudioAnalysis component by changing the fill value on an image.
/// Set up one of these for each band to see the full spectrum.
/// </summary>
public class FrequencyBandDisplay : MonoBehaviour
{
    private const int numBands = 8;

    [SerializeField]
    [FormerlySerializedAs("_audioAnalysis")]
    [Tooltip("AudioAnalysis component to show data from")]
    private AudioAnalysis audioAnalysis;

    [SerializeField]
    [FormerlySerializedAs("_band")]
    [Tooltip("Frequency band to show")]
    [Range(0, 7)]
    private int band;

    [SerializeField]
    [FormerlySerializedAs("_tinted")]
    [Tooltip("Use AudioAnalysis Bass, Mid, and High colors instead of image color")]
    private bool tinted = false;

    [SerializeField]
    [Tooltip("Band values are multiplied by this constant. Increase this if the audio is quiet or if the bands do not look \"exciting\" enough")]
    private int multiplier = 4;

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
        image.fillAmount = audioAnalysis.AudioBandBuffer[band] * multiplier;
    }
}
