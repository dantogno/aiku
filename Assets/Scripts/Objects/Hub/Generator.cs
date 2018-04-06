using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls the generator's behaviors.
/// The class is applied to the generator.
/// </summary>

public class Generator : MonoBehaviour
{
    public static event Action Exploded;

    [SerializeField, Tooltip("When the player has completed, the generator should start smoking.")]
    private Task valveTask;

    [SerializeField, Tooltip("Particle effects.")]
    private GameObject smoke, explosion;

    [SerializeField, Tooltip("The animator component for the generator.")]
    private Animator generatorAnimator;

    private void OnEnable()
    {
        valveTask.OnTaskCompleted += Smoke;
        EngineSequenceManager.OnShutdown += Explode;
    }

    private void OnDisable()
    {
        valveTask.OnTaskCompleted -= Smoke;
        EngineSequenceManager.OnShutdown -= Explode;
    }

    /// <summary>
    /// Emit smoke and speed up.
    /// </summary>
    private void Smoke()
    {
        smoke.SetActive(true);
        generatorAnimator.SetTrigger("SpeedUp");
    }

    /// <summary>
    /// What the name says, baby.
    /// </summary>
    private void Explode()
    {
        explosion.SetActive(true);
        smoke.SetActive(false);

        generatorAnimator.SetTrigger("Die");

        if (Exploded != null) Exploded.Invoke();

        // After three seconds, stop exploding.
        Invoke("StopExploding", 3);
    }

    /// <summary>
    /// If this is not called, the animation will loop.
    /// </summary>
    private void StopExploding()
    {
        explosion.SetActive(false);
    }
}
