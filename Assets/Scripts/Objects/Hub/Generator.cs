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

    [SerializeField]
    private Task valveTask;

    [SerializeField]
    private GameObject smoke, explosion;

    [SerializeField]
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

    private void Smoke()
    {
        smoke.SetActive(true);
        generatorAnimator.SetTrigger("SpeedUp");
    }

    private void Explode()
    {
        explosion.SetActive(true);
        smoke.SetActive(false);

        generatorAnimator.SetTrigger("Die");

        if (Exploded != null) Exploded.Invoke();

        Invoke("StopExploding", 3);
    }

    void StopExploding()
    {
        explosion.SetActive(false);
    }
}
