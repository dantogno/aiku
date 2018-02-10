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

