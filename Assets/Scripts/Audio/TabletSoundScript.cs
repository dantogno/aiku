using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletSoundScript : MonoBehaviour {

    ///This is the sound trigger for the tablet. 
    ///Drag any gameObject to the slot in the inspector 
    ///Place this script on the object you wish to monitor. 
    ///Sound will play on the object this script is on. 

    //AudioSource to play sound
    private AudioSource audioSource;

    //References player
    [SerializeField]
    private GameObject player;
    //References roomba 
    [SerializeField]
    private GameObject roomba;

    //CustomController of player
    private CustomRigidbodyFPSController playerController;
    //CustomController of roomba
    private CustomRigidbodyFPSController roombaController;

    //Distance this gameobject is from the reference point. 
    [SerializeField]
    private float distance;

    //Max distance before sound effect will play
    [SerializeField]
    private float distanceToTriggerSound = 10;

    //Starts true and returns false once this distance exceeds distanceToTriggerSound
    [SerializeField]
    private bool canPlaysound;

    void Start ()
    {
        playerController = player.GetComponent<CustomRigidbodyFPSController>();
        roombaController = roomba.GetComponent<CustomRigidbodyFPSController>();

        audioSource = GetComponent<AudioSource>();     
        canPlaysound = true;
        distance = Vector3.Distance(transform.position, transform.position);
	}
	
	// Update is called once per frame
	void Update () {

		if(!playerController.enabled && !roombaController.enabled && canPlaysound)
        {
            audioSource.Play();
            canPlaysound = false;
        }
	}
}
