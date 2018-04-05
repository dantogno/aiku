using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Flashes an image in time with the overall volume of an AudioSource
/// </summary>
public class AmplitudeBrightnessModulation : MonoBehaviour {

    [SerializeField]
    private AudioAnalysis audioAnalysis;

    [SerializeField]
    [Tooltip("The image to flash")]
    private Image screenImage;

    [SerializeField]
    [Tooltip("The color when no sound is playing (0: black -> 1: always full brightness)")]
    private float minBrightness;

    protected Color color;

	// Use this for initialization
	private void Start () {
        color = screenImage.color;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        Color newColor = Color.Lerp(Color.black, color, audioAnalysis.AmplitudeBuffer + minBrightness);
        screenImage.color = newColor;
	}
}