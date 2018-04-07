using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlaceMinerals : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject crystal;
    [SerializeField] private GameObject TheEnd;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject transition;
    [SerializeField] private SceneChanger transitionScript;

    public void Interact(GameObject agent)
    {
        crystal.SetActive(true);
        TheEnd.SetActive(true);
        arrow.SetActive(false);
        transition.SetActive(true);
        transitionScript.enabled = true;
    }
}
