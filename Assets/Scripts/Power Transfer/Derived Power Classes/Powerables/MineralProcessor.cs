using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for the mineral processor, a magical device which turns minerals into gold. I mean power.
/// The script is applied to the (you guessed it) mineral processor.
/// </summary>

public class MineralProcessor : PowerableObject
{
    [SerializeField, Tooltip("The minerals which can make power.")]
    private Collider minerals;

    private AudioSource myAudioSource;

    private void OnTriggerEnter(Collider other)
    {
        // If minerals come near, gobble them up and make power out of them.
        if (other == minerals) ProcessMinerals();
    }

    private void ProcessMinerals()
    {
        if (!IsFullyPowered)
        {
            // Destroy the minerals rather than deactivate them, to avoid forcing the player
            // to carry around a useless deactivated GameObject.
            Destroy(minerals.gameObject);

            // Now that the minerals have been destroyed, generate power from their disintegration.
            base.PowerOn();

            if (GetComponentInChildren<AudioSource>() != null)
            {
                myAudioSource = GetComponentInChildren<AudioSource>();
                myAudioSource.Play();
            }
        }
    }
}
