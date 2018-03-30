using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void MoveCrystal()
    {
        if (animationPlayed && gameObject.transform.position != gmd.transform.position)
        {
           
            
            //Debug.Log("Fuck off");
            float step = 4 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(gameObject.transform.position, gmd.transform.position, step);

            //gameObject.transform.Translate(gmd.transform.position);
            //new WaitForSeconds(1);
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
    private void MineMinerals(int i)
    {
        //Debug.Log("Suh Dud");
        animator.SetBool("playRotationJiggle", true);
        StartCoroutine(Jiggle());
        portalToActivate.SetActive(true);
    }

    IEnumerator Jiggle()
    {
        yield return new WaitForSeconds(3);
        animationPlayed = true;
    }
   
}
