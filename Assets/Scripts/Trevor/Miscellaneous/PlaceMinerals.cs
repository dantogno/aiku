using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlaceMinerals : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject crystal;
    [SerializeField] private GameObject arrow;
	[SerializeField] private GameObject monitorArrow;

	public static event Action PlacedMinerals;

    public void Interact(GameObject agent)
    {
        crystal.SetActive(true);
        arrow.SetActive(false);
		monitorArrow.SetActive (true);
    }
}
