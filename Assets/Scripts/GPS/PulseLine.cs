using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the movement and opacity of the GPSLine material
/// </summary>
public class PulseLine : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Opacity of the line over time during a pulse")]
    private AnimationCurve PulseCurve;

    [SerializeField]
    [Tooltip("Speed to trace along the line")]
    private float LineMoveSpeed;

    [SerializeField]
    [Tooltip("Time between pulses")]
    private float pulseTime;

    [SerializeField]
    [Tooltip("Line renderer with GPSLineShader material on it")]
    private LineRenderer GPSLineRenderer;

    private float pulseStartTime = float.MaxValue;

	// Use this for initialization
	private void Start ()
    {
        Pulse();
	}
	
	// Update is called once per frame
	private void Update ()
    {
        UpdateGPSLineTexture();
	}

    private void PulseOnce()
    {
        pulseStartTime = Time.time;
    }

    /// <summary>
    /// Starts the line pulsing
    /// </summary>
    public void Pulse()
    {
        InvokeRepeating("PulseOnce", 0, pulseTime);
    }

    /// <summary>
    /// Cancels the pulse (the current one will play through to the end)
    /// </summary>
    public void StopPulse()
    {
        CancelInvoke("PulseOnce");
    }

    /// <summary>
    /// change the opacity and "trace" the line by changing the texture offset
    /// </summary>
    private void UpdateGPSLineTexture()
    {
        Vector2 offset = GPSLineRenderer.material.mainTextureOffset;

        offset.x += Time.deltaTime * LineMoveSpeed;
        if (offset.x >= 1)
            offset.x = 0;

        //set offset
        GPSLineRenderer.material.mainTextureOffset = offset;

        //set opacity
        GPSLineRenderer.material.SetFloat("_Opacity", PulseCurve.Evaluate(Time.time - pulseStartTime));
    }
}
