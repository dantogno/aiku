using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for lights which should turn off or on when their associated powerable is powered off or on.
/// The script can be applied to anything with at least one light as a child.
/// </summary>

public class LightConnectedToPowerable : MonoBehaviour
{
    [SerializeField, Tooltip("The powerable which powers the light or lights.")]
    private PowerableObject powerable;

    [SerializeField, Tooltip("The color of the light or lights.")]
    private Color originalColor = Color.white, offColor = Color.black;

    [SerializeField, Tooltip("The starting intensity for the light or lights.")]
    private float originalIntensity = 1;

    private void OnEnable()
    {
        powerable.OnPoweredOff += TurnOff;
        powerable.OnPoweredOn += TurnOn;
    }

    private void OnDisable()
    {
        powerable.OnPoweredOff -= TurnOff;
        powerable.OnPoweredOn -= TurnOn;
    }

    /// <summary>
    /// Turn all child lights off.
    /// </summary>
    private void TurnOff()
    {
        // Turn off emission if the light is emissive.
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.SetColor("_EmissionColor", offColor);
        }

        // Turn the intensity down to zero if the light is a standard Unity light.
        foreach (Light l in GetComponentsInChildren<Light>())
        {
            l.intensity = 0;
            l.color = offColor;
        }
    }

    /// <summary>
    /// Turn all child lights on.
    /// </summary>
    private void TurnOn()
    {
        // Turn on emission if the light is emissive.
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.SetColor("_EmissionColor", originalColor);
        }

        // Turn the intensity up to the original intensity if the light is a standard Unity light.
        foreach (Light l in GetComponentsInChildren<Light>())
        {
            l.intensity = originalIntensity;
            l.color = originalColor;
        }
    }
}
