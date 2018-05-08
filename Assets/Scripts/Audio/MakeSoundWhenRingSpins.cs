using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class plays a sound when the player spins the hologenerator rings.
/// The script is applied to a prefab which should be added into the hologenerator asset's hierarchy.
/// </summary>

[RequireComponent(typeof(AudioSource))]
public class MakeSoundWhenRingSpins : MonoBehaviour
{
    private AudioSource myAudioSource;

    private void OnEnable()
    {
        RotateRings.RotatedRings += PlaySound;
    }
    private void OnDisable()
    {
        RotateRings.RotatedRings -= PlaySound;
    }

    /// <summary>
    /// Play a sound when a ring is rotated.
    /// </summary>
    public void PlaySound()
    {
        myAudioSource = GetComponent<AudioSource>();

        if (myAudioSource != null) myAudioSource.Play();
    }
}
