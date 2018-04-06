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
        }
    }
}

  
