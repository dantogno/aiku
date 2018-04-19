using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoorTrigger : MonoBehaviour {

    //[SerializeField]
    //private GameObject door;

    [SerializeField]
    private GameObject doorOriginPoint;

    [SerializeField]
    private GameObject doorEndPoint;

    [SerializeField]
    private float countDownToDisappear;

    [SerializeField]
    private float timeToAutoClose;

    [SerializeField]
    private bool accessGranted;

    private void Start()
    {
      
    }

    void Update()
    {
        if (accessGranted)
        {
            GrantAccess();
            // light.color.g;
        }
        //if (door.activeSelf == false)
        //{
        //    AutoCloseDoor();
        //}
        else
        {
            DenyAccess();
        }
    }

    private void OnTriggerEnter(Collider gameObject)
    {
        if(gameObject.tag == "Player")
        {
            timeToAutoClose = 2;
            accessGranted = true;
            countDownToDisappear = 3;
        }
    }

    private void OnTriggerExit(Collider gameObject)
    {
        if(gameObject.tag == "Player")
        {
            AutoCloseDoor();
        }
    }

    private void AutoCloseDoor()
    {
        timeToAutoClose -= Time.deltaTime;
        if (timeToAutoClose <= 0)
        {
            accessGranted = false;
            DenyAccess();
        }
    }

    private void GrantAccess()
    {
        countDownToDisappear -= Time.deltaTime;
        //Vector3 startingPosition = new Vector3(door.transform.position.x, door.transform.position.y, door.transform.position.z);
        //door.transform.position = Vector3.Lerp(startingPosition, doorEndPoint.transform.position, Time.deltaTime);
        //if (countDownToDisappear <= 0)
        //{
        //    door.SetActive(false);
        //}
    }

    private void DenyAccess()
    {
        //    door.SetActive(true);
        //    Vector3 startingPosition = new Vector3(door.transform.position.x, door.transform.position.y, door.transform.position.z);
        //    door.transform.position = Vector3.Lerp(startingPosition, doorOriginPoint.transform.position, Time.deltaTime);
        //}
    }
}
