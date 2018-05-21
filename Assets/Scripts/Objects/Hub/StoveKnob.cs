using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class allows the player to turn the stove on.
/// The script is applied to the knobs on the stove.
/// </summary>

public class StoveKnob : MonoBehaviour, IInteractable
{
    [SerializeField, Tooltip("The galley system's powerable object component.")]
    private PowerableObject powerable;

    [SerializeField, Tooltip("The burner associated with this knob.")]
    private Renderer burner;

    [SerializeField, Tooltip("The color we want the burner to glow"), ColorUsage(false, true, 0, 8, 0.125f, 3)]
    private Color glowColor;

    [SerializeField, Tooltip("The time it takes for the burner to get hot or cool down.")]
    private float burnerTimer = 3;

    [SerializeField, Tooltip("The turned Y value of the knob.")]
    private float knobTurnedAngle = 90;

    // These two variables are where we store the orientation of the knob.
    private Vector3 originalEulers, turnedEulers;

    private bool burnerIsGlowing = false, knobIsTurned = false;

    private void OnEnable()
    {
        powerable.OnPoweredOff += TurnBurnerOff;
        powerable.OnPoweredOn += TurnBurnerOnIfKnobIsTurned;
    }
    private void OnDisable()
    {
        powerable.OnPoweredOff -= TurnBurnerOff;
        powerable.OnPoweredOn -= TurnBurnerOnIfKnobIsTurned;
    }

    private void Start()
    {
        originalEulers = transform.eulerAngles;
        turnedEulers = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + knobTurnedAngle, transform.eulerAngles.z);
    }

    public void Interact(GameObject interactingAgent)
    {
        TurnKnob();
        if (powerable.IsFullyPowered) ToggleBurnerGlow();
    }

    /// <summary>
    /// Change the physical orientation of the knob, to indicate that a burner is on.
    /// </summary>
    private void TurnKnob()
    {
        knobIsTurned = !knobIsTurned;

        transform.eulerAngles = knobIsTurned ? turnedEulers : originalEulers;
    }

    /// <summary>
    /// Turn the burner's emissive glow on or off.
    /// </summary>
    private void ToggleBurnerGlow()
    {
        burnerIsGlowing = !burnerIsGlowing;

        StartCoroutine(ChangeBurnerEmissive());
    }

    /// <summary>
    /// When the power is taken away, the burner turns off.
    /// </summary>
    private void TurnBurnerOff()
    {
        burnerIsGlowing = false;

        StartCoroutine(ChangeBurnerEmissive());
    }

    /// <summary>
    /// When power is restored, the burner should turn on if the switch is turned.
    /// </summary>
    private void TurnBurnerOnIfKnobIsTurned()
    {
        if (knobIsTurned)
        {
            burnerIsGlowing = true;

            StartCoroutine(ChangeBurnerEmissive());
        }
    }

    /// <summary>
    /// The burner should glow or stop glowing gradually when it changes.
    /// </summary>
    private IEnumerator ChangeBurnerEmissive()
    {
        Color targetColor = burnerIsGlowing ? glowColor : Color.black,
            originalColor = burner.material.GetColor("_EmissionColor"),
            newColor = originalColor;

        float elapsedTime = 0;
        while (elapsedTime < burnerTimer)
        {
            newColor = Color.Lerp(originalColor, targetColor, elapsedTime / burnerTimer);
            burner.material.SetColor("_EmissionColor", newColor);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        burner.material.SetColor("_EmissionColor", targetColor);
    }
}
