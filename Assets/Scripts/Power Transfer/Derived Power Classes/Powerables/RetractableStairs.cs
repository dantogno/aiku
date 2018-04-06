using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls the behavior of the powerable stairs.
/// It is applied to the two stair systems on the ship.
/// </summary>

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]

public class RetractableStairs : PowerableObject
{
    [SerializeField, Tooltip("This is the sound which plays when the stairs are retracted.")]
    private AudioClip retractClip;

    [SerializeField, Tooltip("This is the sound which plays when the stairs are extended.")]
    private AudioClip extendClip;

    [Header("Stair Colliders")]

    [SerializeField]
    private Collider retractedCollision, extendedCollision, transitionCollision, hydraulicCollision1, hydraulicCollision2;

    private AudioSource myAudioSource;
    private Animator myAnimator;

    private void Awake()
    {
        InitializeReferences();

        // Prevents the stairs from making noise when the game begins.
        myAudioSource.volume = 0;
    }

    private void InitializeReferences()
    {
        myAudioSource = GetComponent<AudioSource>();
        myAnimator = GetComponent<Animator>();
    }

    public override void PowerOn()
    {
        base.PowerOn();

        ExtendStairs();
    }

    public override void PowerOff()
    {
        base.PowerOff();

        RetractStairs();
    }

    /// <summary>
    /// Play the extension animation and sound, and change collision accordingly.
    /// </summary>
    private void ExtendStairs()
    {
        myAnimator.SetTrigger("Extend");

        myAudioSource.clip = extendClip;
        myAudioSource.Play();

        hydraulicCollision1.enabled = true;
        hydraulicCollision2.enabled = true;

        StartCoroutine(EnableCollision(extendedCollision, retractedCollision));
    }

    /// <summary>
    /// Play the rectraction animation and sound, and change collision accordingly.
    /// </summary>
    private void RetractStairs()
    {
        myAudioSource.volume = 1;

        myAnimator.SetTrigger("Retract");

        myAudioSource.clip = retractClip;
        myAudioSource.Play();

        hydraulicCollision1.enabled = false;
        hydraulicCollision2.enabled = false;

        StartCoroutine(EnableCollision(retractedCollision, extendedCollision));
    }

    /// <summary>
    /// Change collision to match the stairs' position.
    /// </summary>
    /// <param name="collisionToEnable"></param>
    /// <param name="collisionToDisable"></param>
    /// <returns></returns>
    private IEnumerator EnableCollision(Collider collisionToEnable, Collider collisionToDisable)
    {
        transitionCollision.enabled = true;
        collisionToEnable.enabled = true;
        collisionToDisable.enabled = false;

        yield return new WaitForSeconds(2);
        
        transitionCollision.enabled = false;
    }
}
