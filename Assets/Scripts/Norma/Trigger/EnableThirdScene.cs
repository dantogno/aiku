using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableThirdScene : MonoBehaviour {

	public static event Action enteredLastScene;

    [SerializeField]
    [Tooltip("drag the orbit3 gameobject here ")]
    private GameObject orbit3;
    [SerializeField]
    [Tooltip("drag the orbit4 gameobject here ")]
    private GameObject orbit4;
    [SerializeField]
    [Tooltip("drag the hallway gameobject here ")]
    private GameObject hallway;

    /// <summary>
    /// upon collision, enable the hallway and disable the old orbits
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        hallway.SetActive(true);
        orbit3.SetActive(false);
        orbit4.SetActive(false);
		if (enteredLastScene != null) enteredLastScene.Invoke();

    }
    void Start () {
        hallway.SetActive(false);
	}
	
}
