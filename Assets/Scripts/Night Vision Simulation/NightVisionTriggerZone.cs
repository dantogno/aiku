using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class activates night vision! Oooooooo!
/// The script is applied to various trigger zones around the ship.
/// If the trigger's ZoneType is Switch, it must be a child of a powerable object.
/// </summary>

public class NightVisionTriggerZone : MonoBehaviour
{
    [SerializeField, Tooltip("Will the player activate night vision by walking into a trigger or by taking power from a switch?")]
    private ZoneType zoneType = ZoneType.Trigger;

    private NightVision nightVision;
    private GlitchEffect glitchEffect;
    private PowerableObject connectedPowerable;

    private enum ZoneType { Trigger, Switch }

    private bool generatorBlewUp = false;

    private bool nightVisionIsOn
    {
        get
        {
            return nightVision.enabled;
        }
        set
        {
            if (nightVision.enabled == value)
                return;
            nightVision.enabled = value;
            if (value)
                nightVision.StartScanning();
            else
                nightVision.StartScanning();
        }
        
    }

    private void Awake()
    {
        // There should be only one NighVision script in the hub level, and it's applied to the main camera.
        nightVision = Camera.main.GetComponent<NightVision>();

        // The main camera also has a glitch effect script.
        glitchEffect = Camera.main.GetComponent<GlitchEffect>();

        if (zoneType == ZoneType.Switch) connectedPowerable = GetComponentInParent<PowerableObject>();
    }

    private void OnEnable()
    {
        EngineSequenceManager.OnShutdown += WaitToAllowNightVision;
        if (connectedPowerable != null) connectedPowerable.OnPoweredOff += TurnOnNightVision;
    }
    private void OnDisable()
    {
        EngineSequenceManager.OnShutdown -= WaitToAllowNightVision;
        if (connectedPowerable != null) connectedPowerable.OnPoweredOff -= TurnOnNightVision;
    }

    private void OnTriggerStay(Collider other)
    {
        bool canActivateNightVision =
            other.tag == "Player" && generatorBlewUp && zoneType == ZoneType.Trigger;

        // When the player is inside a trigger, activate night vision.
        if (canActivateNightVision && !nightVisionIsOn)
        {
            TurnOnNightVision();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            TurnOffNightVision();
        }
    }

    /// <summary>
    /// After the generator explodes, the player can use night vision after we wait for the other powerables to activate.
    /// </summary>
    private void WaitToAllowNightVision()
    {
        Invoke("AllowNightVision", 1);
    }

    /// <summary>
    /// The player can now use night vision.
    /// </summary>
    private void AllowNightVision()
    {
        generatorBlewUp = true;

        if (glitchEffect != null)
        {
            glitchEffect.enabled = false;
        }
    }

    /// <summary>
    /// Turn on night vision!
    /// </summary>
    private void TurnOnNightVision()
    {
        if (nightVision != null && glitchEffect != null && generatorBlewUp)
        {
            // The glitch effect script interferes with the night vision's rendering, so we turn it off.
            glitchEffect.enabled = false;

            nightVisionIsOn = true;
        }
    }

    /// <summary>
    /// Turn off night vision!
    /// </summary>
    private void TurnOffNightVision()
    {
        if (nightVision != null && glitchEffect != null)
        {
            // We are not using night vision anymore, so we 
            glitchEffect.enabled = true;

            // We disable the script, because it may interfere with glitchiness.
            nightVisionIsOn = false;
        }
    }
}
