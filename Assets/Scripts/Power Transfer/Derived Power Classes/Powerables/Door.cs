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

    [Tooltip("The time it takes for the emissives to light up or fade away")]
    [SerializeField]
    private float lightTimer = 1;

    [Tooltip("If this box is checked, the door will not open.")]
    [SerializeField]
    private bool locked = false;

    private Animator myAnimator;

    private AudioSource myAudioSource;

    private float originalPitch;
    private bool open = false;

    protected override void Start()
    {
        base.Start();

        myAnimator = GetComponent<Animator>();
        myAudioSource = GetComponent<AudioSource>();
        originalPitch = myAudioSource.pitch;
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
        //hack for solving scanner issue
        if (other.isTrigger)
            return;

        bool canOpen = IsFullyPowered && !open && !locked;

        if (canOpen) OpenDoor();
    }

    private void OnTriggerExit(Collider other)
    {
        //hack for solving scanner issue
        if (other.tag == "Player")
        {
            bool canClose = open && !locked;

            if (canClose) CloseDoor();
        }
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
        myAudioSource.pitch = Random.Range(originalPitch - .1f, originalPitch + .1f);

        myAudioSource.clip = openClip;
        myAudioSource.Play();
    }

    private void PlayClosedSound()
    {
        myAudioSource.pitch = Random.Range(originalPitch - .1f, originalPitch + .1f);

        myAudioSource.clip = closeClip;
        myAudioSource.Play();
    }

    /// <summary>
    /// TODO: Not working.
    /// </summary>
    /// <param name="on"></param>
    /// <returns></returns>
    private IEnumerator ChangeEmissiveColor(bool on)
    {
        Renderer emissiveRenderer = transform.GetChild(0).GetComponent<Renderer>();

        Color originalColor = emissiveRenderer.material.GetColor("_EmissionColor"),
                targetColor = on ? Color.white : Color.clear,
                newColor = originalColor;
        
        float elapsedTime = 0;
        while (elapsedTime < lightTimer)
        {
            newColor = Color.Lerp(originalColor, targetColor, elapsedTime / lightTimer);
            emissiveRenderer.material.SetColor("_EmissionColor", newColor);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        yield return new WaitForEndOfFrame();
        emissiveRenderer.material.SetColor("_EmissionColor", targetColor);
    }

    public void LockDoor()
    {
        locked = true;
        ChangeEmissiveColor(false);
    }

    public void UnlockDoor()
    {
        locked = false;
        ChangeEmissiveColor(true);
    }
}
