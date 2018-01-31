using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A power switch is a device found as a child object on most powerable objects.
/// The power switch features the ability to interact with most powerable objects.
/// It also lights up any emissive materials in its child objects to indicate its connected powerable object's power state.
/// Power switches transfer power all at once, and cannot transfer power incrementally.
/// A power switch must be a child of a powerable object to function correctly.
/// </summary>

[RequireComponent(typeof(AudioSource))]

public class PowerSwitch : PowerExchanger
{
    // GameObjects cannot interact with a blocked power switch.
    public bool Blocked { get { return blocked; } }

    #region These are the different colors that indicate to the player whether a powerable object has power, and what its power level is. The color values are taken from the game's official color palette.

    [Tooltip("Color indicating that the connected powerable is off.")]
    [SerializeField]
    [ColorUsage(false, true, 0, 8, 0.125f, 3)]
    private Color offColor = new Color(0.7019608f, 0f, 0.3294118f);

    [Tooltip("Color indicating that the connected powerable is on.")]
    [SerializeField]
    [ColorUsage(false, true, 0, 8, 0.125f, 3)]
    private Color onColor = new Color(0.1215686f, 0.9254903f, 1);

    [Tooltip("Color indicating that the connected powerable is blocked.")]
    [SerializeField]
    [ColorUsage(false, true, 0, 8, 0.125f, 3)]
    private Color blockedColor = new Color(1, 0.7686275f, 0.1372549f);

    [Tooltip("Color indicating a 'blank' or undefined state; no color visible on the indicator light.")]
    [SerializeField]
    [ColorUsage(false, true, 0, 8, 0.125f, 3)]
    private Color noColor = Color.black;

    #endregion

    #region These audio clips represent the different sound effects that play when a power exchange is attempted.

    [Tooltip("Sound that plays when a power switch is activated.")]
    [SerializeField]
    private AudioClip powerOnAudioClip;

    [Tooltip("Sound that plays when a power switch is deactivated.")]
    [SerializeField]
    private AudioClip powerOffAudioClip;

    [Tooltip("Sound that plays when the player tries to access a blocked power switch.")]
    [SerializeField]
    private AudioClip blockedAudioClip;

    #endregion

    [Tooltip("If this box is checked, the player cannot interact with the power switch.")]
    [SerializeField]
    private bool blocked = false;

    // Power indication renderers are the emissive materials used to indicate a powerable object's power state to the player.
    private List<Renderer> powerIndicationRenderers = new List<Renderer>();

    // The audio source component attached to the power switch, used to play sounds when the player interacts with the switch.
    private AudioSource myAudioSource;

    // The stored color of the power switch's emissive materials, which changes based on the connected powerable's power state.
    private Color currentColor;

    protected override void Awake()
    {
        base.Awake();

        InitializeReferences();
    }

    private void OnEnable()
    {
        connectedPowerable.OnPoweredOn += DisplayOnColor;
        connectedPowerable.OnPoweredOff += DisplayOffColor;
    }
    private void OnDisable()
    {
        connectedPowerable.OnPoweredOn -= DisplayOnColor;
        connectedPowerable.OnPoweredOff -= DisplayOffColor;
    }

    /// <summary>
    /// Transfer power between an interacting agent (otherPowerable) and the switch's connected powerable.
    /// </summary>
    /// <param name="otherPowerable"></param>
    protected override void TransferPower(IPowerable otherPowerable)
    {
        if (!blocked)
        {
            bool otherPowerableHasEnoughPower = otherPowerable.CurrentPower >= connectedPowerable.RequiredPower,
                otherPowerableCanAcceptPower = otherPowerable.CurrentPower + connectedPowerable.CurrentPower <= otherPowerable.RequiredPower;

            // If the interacting agent can withdraw power from the connected powerable...
            if (connectedPowerable.IsFullyPowered && otherPowerableCanAcceptPower)
            {
                AllowOtherPowerableToExtractAllPowerFromConnectedPowerable(otherPowerable);
            }

            // If the connected powerable needs more power and the interacting agent has enough power to fully power it...
            else if (!connectedPowerable.IsFullyPowered && otherPowerableHasEnoughPower)
            {
                ReceivePowerFromOtherPowerable(otherPowerable);
            }

            // Edge case. No power is exchanged, and an error light blinks.
            else
            {
                StartCoroutine(BlinkErrorColor());
            }
        }

        // Edge case. No power is exchanged, and an error light blinks.
        else StartCoroutine(BlinkErrorColor());

        // Play the appropriate sound effect for the power exchange.
        myAudioSource.Play();
    }
    
    /// <summary>
    /// Sets up references to power indicator child components and AudioSource component.
    /// </summary>
    private void InitializeReferences()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            powerIndicationRenderers.Add(r);

        myAudioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// The connected powerable transfers all of its power to otherPowerable.
    /// </summary>
    /// <param name="otherPowerable"></param>
    private void AllowOtherPowerableToExtractAllPowerFromConnectedPowerable(IPowerable otherPowerable)
    {
        otherPowerable.AddPower(connectedPowerable.CurrentPower);
        connectedPowerable.PowerOff();

        // The next sound to play should be the power-off clip, since the connected powerable is powering off.
        myAudioSource.clip = powerOffAudioClip;
    }

    /// <summary>
    /// OtherPowerable transfers enough power to fully power the connected powerable.
    /// </summary>
    /// <param name="otherPowerable"></param>
    private void ReceivePowerFromOtherPowerable(IPowerable otherPowerable)
    {
        otherPowerable.SubtractPower(connectedPowerable.RequiredPower);
        connectedPowerable.PowerOn();

        // The next sound to play should be the power-on clip, since the connected powerable is powering on.
        myAudioSource.clip = powerOnAudioClip;
    }

    /// <summary>
    /// When the player unsuccessfully attempts to interact with a power switch, it blinks at them.
    /// </summary>
    /// <returns></returns>
    private IEnumerator BlinkErrorColor()
    {
        // The next sound to play should be the error clip, since if this method was called, the user's interaction with the power switch has been unsuccessful.
        myAudioSource.clip = blockedAudioClip;

        Color originalColor = currentColor;
        float blinkTime = .1f;

        #region Flash off and on.

        int numBlinks = 5;
        for (int i = 0; i < numBlinks; i++)
        {
            SetPowerLightMaterialColor(offColor);
            yield return new WaitForSeconds(blinkTime);
            SetPowerLightMaterialColor(noColor);
            yield return new WaitForSeconds(blinkTime);
        }

        SetPowerLightMaterialColor(originalColor);

        #endregion
    }

    /// <summary>
    /// Sets each of the power indication materials to the appropriate color.
    /// </summary>
    /// <param name="color"></param>
    private void SetPowerLightMaterialColor(Color color)
    {
        foreach (Renderer r in powerIndicationRenderers)
            r.material.SetColor("_EmissionColor", color);
    }

    /// <summary>
    /// When the connected powerable powers on, change the emissive material color accordingly.
    /// If the power switch is blocked, convey this to the player.
    /// </summary>
    private void DisplayOnColor()
    {
        Color displayColor = blocked ? blockedColor : onColor;

        SetPowerLightMaterialColor(displayColor);
        currentColor = displayColor;
    }

    /// <summary>
    /// When the connected powerable powers off, change the emissive material color accordingly.
    /// </summary>
    private void DisplayOffColor()
    {
        SetPowerLightMaterialColor(offColor);

        if (connectedPowerable.IsFullyPowered) StartCoroutine(BlinkErrorColor());
        else
        {
            myAudioSource.clip = powerOffAudioClip;
            myAudioSource.Play();
        }

        currentColor = offColor;
    }
}
