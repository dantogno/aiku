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
    [Tooltip("Invisible collider to keep the player from falling out of the elevator while it's moving.")]
    [SerializeField]
    private Collider elevatorInvisibleBarrierCollider;

    private PowerableObject myPowerable;
    private Collider myTrigger;
    private Animator myAnimator;

    private void OnEnable()
    {
        myPowerable.OnPoweredOn += EnableTrigger;
        myPowerable.OnPoweredOff += DisableColliders;
    }
    private void OnDisable()
    {
        myPowerable.OnPoweredOn -= EnableTrigger;
        myPowerable.OnPoweredOff -= DisableColliders;
    }

    private void Awake()
    {
        InitializeReferences();
    }

    private void OnTriggerEnter(Collider other)
    {
        ActivateElevator(other);
    }

    /// <summary>
    /// Initialize references to components.
    /// </summary>
    private void InitializeReferences()
    {
        myPowerable = GetComponent<PowerableObject>();

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
        myAnimator.SetTrigger("Ascend");

        // Deactivate trigger to prevent accidental triggering.
        myTrigger.enabled = false;

        // Activate invisible collider to prevent accidentally falling out of the elevator.
        elevatorInvisibleBarrierCollider.enabled = true;

        // The passenger becomes a child of the platform to prevent an unpleasant "bouncing" physics effect.
        passenger.transform.SetParent(transform);
    }

    /// <summary>
    /// When the elevator is turned on, the trigger is activated.
    /// </summary>
    private void EnableTrigger()
    {
        myTrigger.enabled = true;
    }

    /// <summary>
    /// When the elevator is turned off, the trigger and invisible collider are deactivated.
    /// </summary>
    private void DisableColliders()
    {
        myTrigger.enabled = false;
        elevatorInvisibleBarrierCollider.enabled = false;
    }
}
