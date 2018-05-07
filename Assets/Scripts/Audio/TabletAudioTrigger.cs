using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletAudioTrigger : MonoBehaviour, IInteractable {

    /// Drag the Game Objects to the heirarchy for the corresponding controllers. 
    /// This script checks to see if both CustomRigidBodyFPSControllers are active. 
    /// If both are inactive play sound affect. 

    //AudioSource for sound effect. 
    [SerializeField]
    private AudioSource audioSource;  
    //Bool to check if sound can be played. 
    [SerializeField]
    private bool canPlaySound;

    public void Interact(GameObject agent)
    {
        audioSource.Play();
        canPlaySound = true;

    }
    private void Start ()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        canPlaySound = false;
	}
    private void Update ()
    {
        
	}
}
