using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script behaves like PowerSwitch.cs, but does not actually power anything.
/// The script is applied to power switches found in the character levels, and are used to allow the player to exit these levels.
/// </summary>

public class FakePowerSwitch : MonoBehaviour, IInteractable
{
    public static event Action<Crewmember> GavePowerToCrewmember;

    public enum Crewmember { Norma, Trevor, Ray }

    // Used to count how many levels the player has entered.
    public static int FakeSwitchesActivated { get; private set; }

    [Tooltip("Color indicating that the connected powerable is off.")]
    [SerializeField]
    [ColorUsage(false, true, 0, 8, 0.125f, 3)]
    private Color offColor = new Color(0.7019608f, 0f, 0.3294118f);

    [Tooltip("Color indicating that the connected powerable is on.")]
    [SerializeField]
    [ColorUsage(false, true, 0, 8, 0.125f, 3)]
    private Color onColor = new Color(0.1215686f, 0.9254903f, 1);

    [Tooltip("Color indicating a 'blank' or undefined state; no color visible on the indicator light.")]
    [SerializeField]
    [ColorUsage(false, true, 0, 8, 0.125f, 3)]
    private Color noColor = Color.black;
    [Tooltip("Sound that plays when a power switch is activated.")]
    [SerializeField]
    private AudioClip powerOnAudioClip;

    [SerializeField, Tooltip("Interactable monitor - used as a portal back to the hub level.")]
    private GameObject levelTransitionMonitor;

    [SerializeField, Tooltip("Non-interactable monitor - this monitor is switched out with the level transition monitor when the player has interacted with the power switch.")]
    private GameObject ordinaryMonitor;

    [SerializeField, Tooltip("The crewmember associated with the current level.")]
    private Crewmember crewmember;

    // Power indication renderers are the emissive materials used to indicate a powerable object's power state to the player.
    private List<Renderer> emissiveRenderers = new List<Renderer>();

    // The audio source component attached to the power switch, used to play sounds when the player interacts with the switch.
    private AudioSource myAudioSource;

    // The stored color of the power switch's emissive materials, which changes based on the connected powerable's power state.
    private Color currentColor;

    // Pitch changes a little bit each interaction, for variety.
    private float originalPitch;

    private void Awake()
    {
        InitializeReferences();
    }

    /// <summary>
    /// Transfer power between an interacting agent (otherPowerable) and the switch's connected powerable.
    /// </summary>
    /// <param name="otherPowerable"></param>
    public void Interact(GameObject otherPowerable)
    {
        DisplayOnColor();
        TwistHandle();
        PlaySound();

        ActivateMonitor();

        if (GavePowerToCrewmember != null) GavePowerToCrewmember(crewmember);

        FakeSwitchesActivated++;

        // Disallow interaction once the switch is "powered".
        GetComponent<Collider>().enabled = false;
    }

    /// <summary>
    /// Sets up references to power indicator child components and AudioSource component.
    /// </summary>
    private void InitializeReferences()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            if (r.material.HasProperty("_EmissionColor"))
                emissiveRenderers.Add(r);
        }

        myAudioSource = GetComponent<AudioSource>();
        originalPitch = myAudioSource.pitch;

        SetPowerLightMaterialColor(offColor);
        TwistHandle();
        levelTransitionMonitor.SetActive(false);
        ordinaryMonitor.SetActive(true);
    }

    /// <summary>
    /// Play animation for twisting the power switch's handle.
    /// </summary>
    private void TwistHandle()
    {
        Animator switchAnim = GetComponent<Animator>();
        switchAnim.SetTrigger("Twist");
    }

    /// <summary>
    /// Play sound with random pitching.
    /// </summary>
    private void PlaySound()
    {
        myAudioSource.clip = powerOnAudioClip;
        myAudioSource.pitch = UnityEngine.Random.Range(originalPitch - .1f, originalPitch + .1f);
        myAudioSource.Play();
    }

    /// <summary>
    /// Switch out the non-interactable monitor with the interactable one.
    /// </summary>
    private void ActivateMonitor()
    {
        levelTransitionMonitor.SetActive(true);
        ordinaryMonitor.SetActive(false);
    }

    private void SetPowerLightMaterialColor(Color targetColor)
    {
        foreach (Renderer r in emissiveRenderers)
        {
            r.material.SetColor("_EmissionColor", targetColor);
        }
    }

    /// <summary>
    /// Sets each of the power indication materials to the appropriate color.
    /// </summary>
    /// <param name="targetColor"></param>
    /// <returns></returns>
    private void SetPowerLightMaterialColor(Color targetColor, float timer)
    {
        foreach (Renderer r in emissiveRenderers)
        {
            StartCoroutine(LerpColor(r, targetColor, timer));
        }
    }

    /// <summary>
    /// Fade emissive lights on.
    /// </summary>
    /// <param name="r"></param>
    /// <param name="targetColor"></param>
    /// <param name="timer"></param>
    /// <returns></returns>
    private IEnumerator LerpColor(Renderer r, Color targetColor, float timer)
    {
        Color originalColor = r.material.GetColor("_EmissionColor"),
                newColor = originalColor;

        float elapsedTime = 0;
        while (elapsedTime < timer)
        {
            newColor = Color.Lerp(originalColor, targetColor, elapsedTime / timer);
            r.material.SetColor("_EmissionColor", newColor);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        r.material.SetColor("_EmissionColor", targetColor);
    }

    /// <summary>
    /// When the connected powerable powers on, change the emissive material color accordingly.
    /// If the power switch is blocked, convey this to the player.
    /// </summary>
    private void DisplayOnColor()
    {
        Color displayColor = onColor;

        // The length of the emissive animation is the length of the sound clip.
        SetPowerLightMaterialColor(displayColor, powerOnAudioClip.length);
        currentColor = displayColor;
    }
}
