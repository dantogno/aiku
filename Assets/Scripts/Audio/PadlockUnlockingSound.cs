using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class plays a sound when the hub padlock is unlocked.
/// The script is applied to a child of the padlock.
/// </summary>

public class PadlockUnlockingSound : MonoBehaviour
{
    [SerializeField]
    private LockInteract padlock;

    private void OnEnable()
    {
        padlock.Unlocked += PlaySound;
    }
    private void OnDisable()
    {
        padlock.Unlocked -= PlaySound;
    }

    private void PlaySound()
    {
        AudioSource source = GetComponent<AudioSource>();

        if (source != null) source.Play();
    }
}
