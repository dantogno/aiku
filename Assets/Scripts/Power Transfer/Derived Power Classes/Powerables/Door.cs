using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Doors in this game are automatic. When powered, the door will activate via player proximity.
/// The logic and data within this class are considered to be mostly self-explanatory, hence the lack of documentation.
/// </summary>

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]

public class Door : PowerableObject
{
    [Tooltip("The sound the door makes when it opens")]
    [SerializeField]
    private AudioClip openClip;

    [Tooltip("The sound the door makes when it opens")]
    [SerializeField]
    private AudioClip closeClip;

    [Tooltip("If this box is checked, the door will not open.")]
    [SerializeField]
    private bool locked = false;

    private Animator myAnimator;

    private AudioSource myAudioSource;

    private bool open = false;

    protected override void Start()
    {
        base.Start();

        myAnimator = GetComponent<Animator>();
        myAudioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Sets CurrentPower to zero.
    /// Sets IsFullyPowered to false.
    /// Invokes OnPoweredOff event.
    /// Closes door.
    /// </summary>
    public override void PowerOff()
    {
        base.PowerOff();

        if (!IsFullyPowered && open) CloseDoor();
    }

    private void OnTriggerStay(Collider other)
    {
        bool canOpen = IsFullyPowered && !open && !locked;

        if (canOpen) OpenDoor();
    }

    private void OnTriggerExit(Collider other)
    {
        bool canClose = open && !locked;

        if (canClose) CloseDoor();
    }

    private void OpenDoor()
    {
        myAnimator.SetTrigger("Open");
        PlayOpenSound();
        open = true;
    }

    private void CloseDoor()
    {
        myAnimator.SetTrigger("Close");
        PlayClosedSound();
        open = false;
    }

    private void PlayOpenSound()
    {
        myAudioSource.clip = openClip;
        myAudioSource.Play();
    }

    private void PlayClosedSound()
    {
        myAudioSource.clip = closeClip;
        myAudioSource.Play();
    }

    public void UnlockDoor()
    {
        locked = false;
    }
}
