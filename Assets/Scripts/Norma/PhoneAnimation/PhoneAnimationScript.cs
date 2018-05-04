using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneAnimationScript : MonoBehaviour
{
    [Tooltip("Checks to see if the animation is currently playing")]
    private bool isAnimationPlaying = false;

    [SerializeField]
    [Tooltip("Add the animator for the hand Gameobject")]
    private GameObject handAnimation;

    [SerializeField]
    [Tooltip("Add the animator for the phone sequence")]
    private Animator phoneAnimation;

    private Collider phoneCollider;

    // Use this for initialization
    void Start () {
        handAnimation.SetActive(false);
        phoneAnimation.speed = 0f;
        phoneCollider = GetComponent<Collider>();
	}
	
	void Update ()
    {
        
        if (Time.timeScale == 1.0f )
        {
            if (isAnimationPlaying == true && phoneCollider.isTrigger == false)
            {
                DisablePhoneSequence();
            }

            if (isAnimationPlaying == false && phoneCollider.isTrigger == true)

            {
                EnablePhoneSequence();
            }

        }
    }
    /// <summary>
    /// Enables the Phone Sequence. Unpauses the animation
    /// </summary>
    private void EnablePhoneSequence()
    {
        handAnimation.SetActive(true);
        phoneAnimation.speed = 1f;
        isAnimationPlaying = true;
    }
    /// <summary>
    /// Disables the Phone Sequence. Pauses the animation
    /// </summary>

    private void DisablePhoneSequence()
    {
        handAnimation.SetActive(false);
        phoneAnimation.speed = 0f;
        isAnimationPlaying = false;


    }

}
