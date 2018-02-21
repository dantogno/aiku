using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script triggers the game's end credits.
/// </summary>

[RequireComponent(typeof(BoxCollider))]

public class EndCreditsTriggerPlaceholder : MonoBehaviour
{
    [SerializeField, Tooltip("This is the end credits prefab")]
    EndCredits endCredits;

    private BoxCollider myCollider;

    private void OnEnable()
    {
        EngineSequenceManager.OnShutdown += ActivateCollider;
    }
    private void OnDisable()
    {
        EngineSequenceManager.OnShutdown -= ActivateCollider;
    }

    private void Start()
    {
        InitializeCollider();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Roll the credits when the player enters this object's trigger.
        endCredits.RollCredits();
    }

    /// <summary>
    /// Get the object's collider ready for gameplay.
    /// </summary>
    private void InitializeCollider()
    {
        myCollider = GetComponent<BoxCollider>();

        // Make sure the collider is a trigger.
        if (!myCollider.isTrigger) myCollider.isTrigger = true;

        // Turn the collider off, so that the end credits don't get triggered prematurely.
        myCollider.enabled = false;
    }

    /// <summary>
    /// When the generator shuts down, the trigger is activated.
    /// </summary>
    private void ActivateCollider()
    {
        myCollider.enabled = true;
    }
}
