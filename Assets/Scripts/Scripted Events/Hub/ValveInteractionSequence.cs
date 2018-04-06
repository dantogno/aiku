using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for the valve interaction sequence, which is part of the generator-checking sequence.
/// It is applied to a dedicated GameObject which is a parent of the valves involved in this sequence.
/// </summary>

public class ValveInteractionSequence : MonoBehaviour
{
    public event Action AllThreeValvesTurned, GeneratorBlewUp;

    [Serializable]
    private struct ParticleSpeedPreset
    {
        public float speed, size;
    }

    [SerializeField, Tooltip("Pipe which changes at the beginning of the sequence.")]
    private GameObject normalPipe, BurstPipe;

    [SerializeField, Tooltip("Task involved in the sequence.")]
    private Task fuelCheckTask, manualAnalysisTask;

    [SerializeField, Tooltip("Valves to turn.")]
    private EngineRoomValve[] valves;

    [SerializeField, Tooltip("The big valve which can only be turned after the other three have been turned.")]
    private EngineRoomValve butterflyValve;

    [SerializeField, Tooltip("The steam particle effect coming out of the burst pipe.")]
    private ParticleSystem steam;

    [SerializeField, Tooltip("The settings for the particle system at different stages.")]
    private ParticleSpeedPreset[] particleSpeedPresets;

    // The steam decreases when more valves are closed.
    private int closedValves = 0;

    private void OnEnable()
    {
        foreach (EngineRoomValve valve in valves) valve.ClosedValve += OnValveClosed;
        butterflyValve.ClosedValve += OnButterflyValveClosed;
        fuelCheckTask.OnTaskCompleted += PipeBlewUp;
        manualAnalysisTask.OnTaskCompleted += EnableValveColliders;
    }
    private void OnDisable()
    {
        foreach (EngineRoomValve valve in valves) valve.ClosedValve -= OnValveClosed;
        butterflyValve.ClosedValve -= OnButterflyValveClosed;
        fuelCheckTask.OnTaskCompleted -= PipeBlewUp;
        manualAnalysisTask.OnTaskCompleted -= EnableValveColliders;
    }

    private void Start()
    {
        DisableValveColliders();
        DisableButterflyValveCollider();
    }

    /// <summary>
    /// Disallow interaction by disabling valve colliders.
    /// </summary>
    private void DisableValveColliders()
    {
        foreach (EngineRoomValve valve in valves)
        {
            BoxCollider valveCollider = valve.GetComponent<BoxCollider>();
            valveCollider.enabled = false;
        }
    }

    /// <summary>
    /// Allow interaction by enabling valve colliders.
    /// </summary>
    private void EnableValveColliders()
    {
        foreach (EngineRoomValve valve in valves)
        {
            BoxCollider valveCollider = valve.GetComponent<BoxCollider>();
            valveCollider.enabled = true;
        }
    }

    /// <summary>
    /// Switch out the pipes and activate the particle system.
    /// </summary>
    private void PipeBlewUp()
    {
        normalPipe.SetActive(false);
        BurstPipe.SetActive(true);
        EnableParticleEffect();
    }

    /// <summary>
    /// Turn off the steam.
    /// </summary>
    private void DisableParticleEffect()
    {
        steam.gameObject.SetActive(false);
    }

    /// <summary>
    /// Turn on the steam.
    /// </summary>
    private void EnableParticleEffect()
    {
        steam.gameObject.SetActive(true);
    }

    /// <summary>
    /// The steam has stopped, but now the player's got bigger problems to attend to!
    /// </summary>
    private void OnButterflyValveClosed()
    {
        if (GeneratorBlewUp != null) GeneratorBlewUp();
        DisableParticleEffect();
    }

    /// <summary>
    /// Adjust the steam particle settings when a valve is closed, and count how many valves have been closed.
    /// </summary>
    private void OnValveClosed()
    {
        closedValves++;
        ChangeParticleEmissionSpeed();

        foreach (EngineRoomValve valve in valves)
        {
            if (!valve.Closed) return;
        }

        // If all three valves have been closed, allow the player to interact with the big butterfly valve.
        EnableButterflyValveCollider();
    }

    /// <summary>
    /// Adjust the particle speed and size depending on how many valves have been closed.
    /// </summary>
    private void ChangeParticleEmissionSpeed()
    {
        ParticleSystem.MainModule mainParticleSystem = steam.main;

        switch (closedValves)
        {
            case 0:
                mainParticleSystem.startSpeed = particleSpeedPresets[0].speed;
                mainParticleSystem.startSize = particleSpeedPresets[0].size;
                break;
            case 1:
                mainParticleSystem.startSpeed = particleSpeedPresets[1].speed;
                mainParticleSystem.startSize = particleSpeedPresets[1].size;
                break;
            case 2:
                mainParticleSystem.startSpeed = particleSpeedPresets[2].speed;
                mainParticleSystem.startSize = particleSpeedPresets[2].size;
                break;
            case 3:
                mainParticleSystem.startSpeed = particleSpeedPresets[3].speed;
                mainParticleSystem.startSize = particleSpeedPresets[3].size;
                break;
            case 4:
                mainParticleSystem.startSpeed = particleSpeedPresets[4].speed;
                mainParticleSystem.startSize = particleSpeedPresets[4].size;
                break;
            case 5:
                mainParticleSystem.startSpeed = particleSpeedPresets[5].speed;
                mainParticleSystem.startSize = particleSpeedPresets[5].size;
                DisableParticleEffect();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Allow interaction with butterfly valve by enabling its collider.
    /// </summary>
    private void EnableButterflyValveCollider()
    {
        BoxCollider butterflyValveCollider = butterflyValve.GetComponentInChildren<BoxCollider>();
        butterflyValveCollider.enabled = true;

        if (AllThreeValvesTurned != null) AllThreeValvesTurned.Invoke();
    }

    /// <summary>
    /// Disallow butterfly valve interaction by disabling its collider.
    /// </summary>
    private void DisableButterflyValveCollider()
    {
        BoxCollider butterflyValveCollider = butterflyValve.GetComponentInChildren<BoxCollider>();
        butterflyValveCollider.enabled = false;
    }
}
