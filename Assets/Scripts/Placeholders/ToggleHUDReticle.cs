using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleHUDReticle : MonoBehaviour
{
    [SerializeField] Text reticle;

    Color gray = new Color(.5f, .5f, .5f, .5f);

    private void OnEnable()
    {
        DetectInteractableObject.ObjectToInteractWithChanged += ToggleReticle;
    }
    private void OnDisable()
    {
        DetectInteractableObject.ObjectToInteractWithChanged -= ToggleReticle;
    }

    private void ToggleReticle(IInteractable interactable)
    {
        if (interactable == null) reticle.color = gray;
        else reticle.color = Color.white;
    }
}
