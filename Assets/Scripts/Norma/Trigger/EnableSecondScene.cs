using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableSecondScene : MonoBehaviour {

	public static event Action enteredSecondScene;


    [SerializeField]
    [Tooltip("Drag the third Orbit here")]
    private GameObject rotatedThird;

    [SerializeField]
    [Tooltip("Drag the Fourth Orbit here")]
    private GameObject rotatedFourth;

    [SerializeField]
    [Tooltip("Drag the third Orbit here")]
    private GameObject FirstPuzzleDeletion;

    [SerializeField]
    [Tooltip("Drag the third Orbit here")]
    private GameObject confetti;


    /// <summary>
    /// Upong collision, enable the second act and disable the first puzzle dependencies
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        rotatedFourth.SetActive(true);
        rotatedThird.SetActive(true);
        FirstPuzzleDeletion.SetActive(false);
        confetti.SetActive(false);
		if (enteredSecondScene != null) enteredSecondScene.Invoke();
    }
    void Start () {
        rotatedFourth.SetActive(false);
        rotatedThird.SetActive(false);

    }
		

}
