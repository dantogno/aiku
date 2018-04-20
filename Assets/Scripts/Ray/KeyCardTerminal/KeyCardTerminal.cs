using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls the door beside the keycard terminal.
/// Once a keyCard is placed inside the collider trigger of the gameObject attached to this script. 
/// It will open the door. 
/// </summary>
public class KeyCardTerminal : MonoBehaviour
{

    [SerializeField]
    [Tooltip("The door this opens")]
    private GameObject door;

    [Tooltip("Empty game object that is the door start point.")]
    [SerializeField]
    private GameObject doorOriginPoint;

    [Tooltip("Empty game object that is the door end point.")]
    [SerializeField]
    private GameObject doorEndPoint;

    [Tooltip("Simply checks if conditions are met before granting access.")]
    [SerializeField]
    private bool accessGranted;

    [Tooltip("Time before door game object sets itself inactive while access granted.")]
    [SerializeField]
    private float countDownToDisappear;

    [Tooltip("Time before door moves back to origin point(Closed).")]
    [SerializeField]
    private float timeToAutoClose;

    [Tooltip("Material of the keycard scanner when it's locked")]
    [SerializeField]
    private Material lockedScannerMaterial;

    [Tooltip("Material of the keycard scanner when it's locked")]
    [SerializeField]
    private Material unlockedScannerMaterial;

    [Tooltip("The audio clip that will play when the keycard is scanned")]
    [SerializeField]
    private AudioClip unlockSound;

    //This will be the audio source for unlockSound
    private AudioSource audioSource;

    //Prevents the unlock sound from being played multiple times
    private bool unlockSoundPlayed;

    public void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        audioSource.clip = unlockSound;

        unlockSoundPlayed = false;
    }

    /// <summary>
    /// Update checks for booleans to notify when to perform a method. 
    /// </summary>
    void Update()
    {
        if (accessGranted)
        {
            GrantAccess();
        }
        if (door.activeSelf == false)
        {
            AutoCloseDoor();
        }
        else
        {
            DenyAccess();
        }
    }

    /// <summary>
    /// Once door is opened a timer goes down before it will close the door. 
    /// </summary>
    private void AutoCloseDoor()
    {
        GetComponent<Renderer>().material = lockedScannerMaterial;

        unlockSoundPlayed = false;

        timeToAutoClose -= Time.deltaTime;
        if (timeToAutoClose <= 0)
        {
            accessGranted = false;
            DenyAccess();
        }
    }

    /// <summary>
    /// Will open the door once keycard is placed inside collider trigger and set the door inactive.
    /// </summary>
    private void GrantAccess()
    {
        GetComponent<Renderer>().material = unlockedScannerMaterial;

        countDownToDisappear -= Time.deltaTime;
        Vector3 startingPosition = new Vector3(door.transform.position.x, door.transform.position.y, door.transform.position.z);
        door.transform.position = Vector3.Lerp(startingPosition, doorEndPoint.transform.position, Time.deltaTime);
        if (countDownToDisappear <= 0)
        {
            door.SetActive(false);
        }

    }

    /// <summary>
    /// Default state of door, will remain closed until access is granted. 
    /// </summary>
    private void DenyAccess()
    {
        door.SetActive(true);
        Vector3 startingPosition = new Vector3(door.transform.position.x, door.transform.position.y, door.transform.position.z);
        door.transform.position = Vector3.Lerp(startingPosition, doorOriginPoint.transform.position, Time.deltaTime);
    }

    /// <summary>
    /// If a gameObject with the tag "KeyCard" enters trigger it will set the boolean accessGranted True.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "KeyCard")
        {
            timeToAutoClose = 5;
            accessGranted = true;
            countDownToDisappear = 3;

            if (!unlockSoundPlayed) //Plays the unlock sound only if the door is unlocked
            {
                if (!audioSource.isPlaying)  // Inplace to ensure that our audio keeps looping and dosen't restart each frame input is held down.
                {
                    audioSource.Play();
                    unlockSoundPlayed = true;
                }
            }
        }
    }
}