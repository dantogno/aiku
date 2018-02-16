using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class DoorFan : MonoBehaviour
{
    private PowerableObject connectedPowerable;
    private Animator myAnimator;

    private void OnEnable()
    {
        connectedPowerable.OnPoweredOn += StartFan;
        connectedPowerable.OnPoweredOff += StopFan;
    }
    private void OnDisable()
    {
        connectedPowerable.OnPoweredOn -= StartFan;
        connectedPowerable.OnPoweredOff -= StopFan;
    }

    private void Awake()
    {
        connectedPowerable = transform.parent.GetComponentInChildren<PowerableObject>();
        myAnimator = GetComponent<Animator>();
    }

    private void StartFan()
    {
        myAnimator.SetTrigger("Start");
    }

    private void StopFan()
    {
        myAnimator.SetTrigger("Stop");
    }
}
