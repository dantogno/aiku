using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableEngineScreen : MonoBehaviour, IInteractable
{
    public event System.Action OnInteracted;

    private bool active = false;

    [SerializeField]
    private Animator animator;

    public void Interact(GameObject agent)
    {
        if (active)
        {
            animator.SetTrigger("interactActive");
            active = false;
            animator.SetBool("active", false);
            if(OnInteracted != null)
            {
                OnInteracted.Invoke();
            }

            GetComponent<Collider>().enabled = false;   // DW: this is a note from David about not allowing interaction once something doesn't need it anymore, implemented here as a hack

            GetComponent<AudioSource>().pitch = Random.Range(.8f, 1.2f);
            GetComponent<AudioSource>().Play();     // DW
        }
        else
        {
            animator.SetTrigger("interactInactive");
        }
    }

    public void SetActive()
    {
        active = true;
        animator.SetBool("active", true);
    }

    public void SetInactive()
    {
        active = false;
        animator.SetBool("active", false);
    }

    //TODO: REPLACE THIS WITH ACTUAL IPOWERABLE FUNCTIONALITY
    public void PowerOff()
    {
        animator.SetBool("off", true);
    }

}

