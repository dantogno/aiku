using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script manages the behavior of crystals once they have been mined.
/// </summary>
public class CrystalPickUp : MonoBehaviour
{
    [SerializeField] private GameObject portalToActivate;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject gmd;
    public bool animationPlayed;

    private AudioSource crystalAudio;
    private bool hasPlayedAudio = false;

    private void Start()
    {
        crystalAudio = GetComponent<AudioSource>();
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

    private IEnumerator Jiggle()
    {
        yield return new WaitForSeconds(3);
        animationPlayed = true;
    }
   
}
