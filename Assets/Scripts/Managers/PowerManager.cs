using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is to control the player's ability to interact with powerables before the generator shuts down.
/// It is a manager, and should have its own dedicated object which is easy to find in the hierarchy.
/// </summary>

public class PowerManager : MonoBehaviour
{
    [SerializeField, Tooltip("Doors which we do not want the player to be able to enter until the generator shuts down.")]
    private Door[] doorsToDeactivate;

    [SerializeField, Tooltip("We do not want the player to see the Power UI until the generator shuts down.")]
    private PowerUI powerUI;

    [SerializeField, Tooltip("We do not want the player's scanner to work until the generator shuts down.")]
    private ScanInteractable scanner;

    // All power switches in the level.
    private PowerExchanger[] exchangers;

    private void OnEnable()
    {
        EngineSequenceManager.OnShutdown += EnablePowerSwitches;
    }
    private void OnDisable()
    {
        EngineSequenceManager.OnShutdown -= EnablePowerSwitches;
    }

    private void Start()
    {
        InitializeReferences();
        DisablePowerUI();

        #region We wait one second to disable power switches and turn off doors, so as not to interfere with the Powerables' powering on and off in their own Start() methods.

        Invoke("DisablePowerSwitches", 1);
        Invoke("TurnOffDoors", 1);

        #endregion
    }

    private void InitializeReferences()
    {
        exchangers = FindObjectsOfType<PowerExchanger>();
    }

    /// <summary>
    /// Turn off the player's HUD power bar and scanner component.
    /// </summary>
    private void DisablePowerUI()
    {
        powerUI.DisablePowerUI();
        scanner.enabled = false;
    }

    /// <summary>
    /// Enable the colliders of all power switches, thus allowing interaction.
    /// </summary>
    private void EnablePowerSwitches()
    {
        foreach (PowerExchanger p in exchangers)
        {
            p.GetComponent<Collider>().enabled = true;
        }

        scanner.enabled = true;
    }

    /// <summary>
    /// Disable the colliders (and thus interaction) for all power switches, and darken their emission to make them inconspicuous.
    /// </summary>
    private void DisablePowerSwitches()
    {
        foreach (PowerExchanger p in exchangers)
        {
            p.GetComponent<Collider>().enabled = false;

            // In case the power switch is changing its properties in a coroutine while this method is being called, cut all its processes short.
            p.StopAllCoroutines();

            // Set all the power switch's associated renderers to be dark.
            foreach (Renderer r in p.GetComponentsInChildren<Renderer>())
            {
                if (r.material.HasProperty("_EmissionColor"))
                {
                    r.material.SetColor("_EmissionColor", Color.clear);
                }
            }
        }
    }

    /// <summary>
    /// Power off doors which should be inaccessible until generator shuts down, and stop any of their audio sources from playing.
    /// </summary>
    private void TurnOffDoors()
    {
        foreach (Door d in doorsToDeactivate)
        {
            d.PowerOff();

            foreach (AudioSource a in d.GetComponentsInChildren<AudioSource>())
            {
                a.Stop();
            }
        }
    }
}
