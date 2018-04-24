using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script manages the behavior of crystals once they have been mined.
/// </summary>
public class CrystalPickUp : MonoBehaviour
{
    [Tooltip("The 'Quartz' Animator goes here.")]
    [SerializeField] private Animator animator;

    [Tooltip("The 'GMD' GameObject goes here.")]
    [SerializeField] private GameObject gmd;

    [Tooltip("The 'Rubble Object' GameObject goes here.")]
    [SerializeField] private GameObject rubbleObject;

	[Tooltip("The amount of time it takes to pull the crystal out of the wall")]
	[SerializeField] private float pickupDelayTime = 3f;

    [SerializeField] private GameObject player;

	public static event Action ActivateSecondPortal;

    public bool animationPlayed;

    private Animator rubbleAni;
    private Rigidbody[] rubble;
    private AudioSource crystalAudio;
    private bool hasPlayedAudio = false;
	private bool isTimerRunning = false;
	private float time = 0f;
	private bool shouldMoveCrystal = false;

    private void Start()
    {
        crystalAudio = GetComponent<AudioSource>();
        rubble = rubbleObject.GetComponentsInChildren<Rigidbody>();
        rubbleAni = rubbleObject.GetComponent<Animator>();
    }
    private void Update()
    {
		MoveCrystal ();
		Timer ();
    }

    /// <summary>
    /// Moves the crystal towards the GMD after playing
    /// the jiggle animation and plays the colocted sound effect
    /// before disabling it.
    /// </summary>
    private void MoveCrystal()
    {
		if (shouldMoveCrystal)
        {
            
            EnableRubblePhysics(rubble);
            float step = 15 * Time.deltaTime;
            float scaleSpeed = 5f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, gmd.transform.position, step);
            //Vector3 tarScale = new Vector3(.5f, .5f, .5f);
            //transform.localScale = Vector3.Lerp(transform.localScale, tarScale, scaleSpeed);

            if (gameObject.transform.position == gmd.transform.position)
            {
                GetComponentInChildren<MeshRenderer>().enabled = false;
                GetComponentInChildren<BoxCollider>().enabled = false;

                if (ActivateSecondPortal != null)
                {
                    ActivateSecondPortal.Invoke();
                }

                if (!crystalAudio.isPlaying && !hasPlayedAudio)
                {
                    crystalAudio.Play();
                    hasPlayedAudio = true;
                }
                else if (!crystalAudio.isPlaying && hasPlayedAudio)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnEnable()
    {
        animationPlayed = false;
		GMD.MiningStart += StartTimer;
		GMD.MiningEnd += StopTimer;
    }
    private void OnDisable()
    {
		GMD.MiningStart -= StartTimer;
		GMD.MiningEnd += StopTimer;
    }

    /// <summary>
    /// Plays the Jiggle animation and activates the corresponding
    /// portal.
    /// </summary>
    /// <param name="i"></param>
    private void StartTimer()
    {
		isTimerRunning = true;
		animator.SetBool ("playRotationJiggle", true);
        rubbleAni.SetBool("ShouldJiggle", true);
    }

	private void StopTimer()
	{
		isTimerRunning = false;
		time = 0f;
		animator.SetBool ("playRotationJiggle", false);
        rubbleAni.SetBool("ShouldJiggle", false);
    }

	private void Timer()
	{
		if (isTimerRunning) 
		{
			if (time <= pickupDelayTime) 
			{
				time += Time.deltaTime;
                
            } 
			else if (time >= pickupDelayTime) 
			{
				animator.SetBool ("playRotationJiggle", false);
                rubbleAni.enabled = false;
                
				shouldMoveCrystal = true;
				isTimerRunning = false;
			}
		}
	}

    /// <summary>
    /// Enables physics on debries when the crystal is removed from the wall.
    /// </summary>
    /// <param name="rigidbodies"></param>
    private void EnableRubblePhysics(Rigidbody[] rigidbodies)
    {
        foreach (Rigidbody rbl in rigidbodies)
        {
            rbl.isKinematic = false;
            rbl.useGravity = true;
        }
    }

    private void JiggleRubble(Animator[] rubbleAnimators)
    {
        foreach (Animator ani in rubbleAnimators)
        {
            ani.SetBool("ShouldJiggle", true);
        }
    }

    private void JiggleRubbleStop(Animator[] rubbleAnimators)
    {
        foreach (Animator ani in rubbleAnimators)
        {
            ani.SetBool("ShouldJiggle", false);
        }
    }

    private void RubbleAnimatorExit(Animator[] rubbleAnimators)
    {
        foreach (Animator ani in rubbleAnimators)
        {
            ani.SetBool("Remove", true);
        }
    }
}
