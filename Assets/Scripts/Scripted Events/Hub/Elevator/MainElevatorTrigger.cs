using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is applied to the trigger on the main elevator platform in the cargo bay.
/// When something steps onto the platform, the elevator ascends.
/// </summary>

[RequireComponent(typeof(Animator))]

public class MainElevatorTrigger : MonoBehaviour
{
    [SerializeField, Tooltip("Invisible collider to keep the player from falling out of the elevator while it's moving.")]
    private Collider elevatorInvisibleBarrierCollider;

    [SerializeField, Tooltip("Animator component for the gate at the bottom of the elevator.")]
    private Animator gateAnimator;

    // The powerable elevator.
    private PowerableObject myPowerable;

    private Collider myTrigger;
    private Animator myAnimator;

    private void OnEnable()
    {
        myPowerable.OnPoweredOn += OnElevatorPoweredOn;
        myPowerable.OnPoweredOff += OnElevatorPoweredOff;
    }
    private void OnDisable()
    {
        myPowerable.OnPoweredOn -= OnElevatorPoweredOn;
        myPowerable.OnPoweredOff -= OnElevatorPoweredOff;
    }

    private void Awake()
    {
        InitializeReferences();
    }

    private void Start()
    {
        OnElevatorPoweredOff();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ActivateElevator(other);
        }
    }

    /// <summary>
    /// Initialize references to components.
    /// </summary>
    private void InitializeReferences()
    {
        myPowerable = GetComponentInParent<PowerableObject>();

        foreach (Collider c in GetComponents<Collider>())
            if (c.isTrigger) myTrigger = c;

        myAnimator = GetComponent<Animator>();
    }

    /// <summary>
    /// The elevator will ascend.
    /// </summary>
    /// <param name="passenger"></param>
    private void ActivateElevator(Collider passenger)
    {
        // Elevator ascends.
        myAnimator.SetBool("ascend", true);

        // Deactivate trigger to prevent accidental triggering.
        myTrigger.enabled = false;

        // Activate invisible collider to prevent accidentally falling out of the elevator.
        elevatorInvisibleBarrierCollider.enabled = true;

        gateAnimator.SetTrigger("Close");
        gateAnimator.GetComponent<AudioSource>().Play();

        // The passenger becomes a child of the platform to prevent an unpleasant "bouncing" physics effect.
        passenger.transform.SetParent(transform);

        GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// When the elevator is turned on, the trigger is activated.
    /// </summary>
    private void OnElevatorPoweredOn()
    {
        myTrigger.enabled = true;
        elevatorInvisibleBarrierCollider.enabled = false;
        gateAnimator.SetTrigger("Open");
        gateAnimator.GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// When the elevator is turned off, the trigger and invisible collider are deactivated.
    /// </summary>
    private void OnElevatorPoweredOff()
    {
        myTrigger.enabled = false;
        elevatorInvisibleBarrierCollider.enabled = true;

        GetComponent<AudioSource>().Stop();
    }
}
