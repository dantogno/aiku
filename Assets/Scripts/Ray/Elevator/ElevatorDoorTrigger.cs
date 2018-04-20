using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoorTrigger : MonoBehaviour
{

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

    void Update()
    {
        if (accessGranted)
        {
            GrantAccess();
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
        }
    }

    private void GrantAccess()
    {
        countDownToDisappear -= Time.deltaTime;
    }
}
