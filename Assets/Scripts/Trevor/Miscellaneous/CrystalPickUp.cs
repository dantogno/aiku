using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script manages the behavior of crystals once they have been mined.
/// </summary>
public class CrystalPickUp : MonoBehaviour
{
    [Tooltip("The GameObject that you wish to be the portal goes here.")] 
    [SerializeField] private GameObject portalToActivate;

    [Tooltip("The 'Quartz' Animator goes here.")]
    [SerializeField] private Animator animator;

    [Tooltip("The 'GMD' GameObject goes here.")]
    [SerializeField] private GameObject gmd;

    [Tooltip("The 'Rubble Object' GameObject goes here.")]
    [SerializeField] private GameObject rubbleObject;

    public bool animationPlayed;

    private Rigidbody[] rubble;
    private AudioSource crystalAudio;
    private bool hasPlayedAudio = false;

    private void Start()
    {
        crystalAudio = GetComponent<AudioSource>();
        rubble = rubbleObject.GetComponentsInChildren<Rigidbody>();
    }
    private void Update()
    {
        MoveCrystal();
    }

    /// <summary>
    /// Moves the crystal towards the GMD after playing
    /// the jiggle animation and plays the colocted sound effect
    /// before disabling it.
    /// </summary>
    private void MoveCrystal()
    {
        if (animationPlayed && gameObject.transform.position != gmd.transform.position)
        {
            animator.SetBool("playRotationJiggle", false);
            EnableRubblePhysics(rubble);
            float step = 4 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(gameObject.transform.position, gmd.transform.position, step);

            if (gameObject.transform.position == gmd.transform.position)
            {
                GetComponentInChildren<MeshRenderer>().enabled = false;
                GetComponentInChildren<BoxCollider>().enabled = false;

                if (!crystalAudio.isPlaying && !hasPlayedAudio)
                {
                    crystalAudio.Play();
                    hasPlayedAudio = true;
                }
                else if(!crystalAudio.isPlaying && hasPlayedAudio)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnEnable()
    {
        animationPlayed = false;
        GMD.MiningCrystal += MineMinerals;
    }
    private void OnDisable()
    {
        GMD.MiningCrystal -= MineMinerals;
    }

    /// <summary>
    /// Plays the Jiggle animation and activates the corresponding
    /// portal.
    /// </summary>
    /// <param name="i"></param>
    private void MineMinerals(int i)
    {
        animator.SetBool("playRotationJiggle", true);
        StartCoroutine(Jiggle());
        portalToActivate.SetActive(true);
    }

    /// <summary>
    /// Makes the crystal wait for 3 seconds before moving toward the player.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Jiggle()
    {
        yield return new WaitForSeconds(3);
        animationPlayed = true;
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
   
}
