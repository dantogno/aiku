using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script goes on any object that is intended to spin on the Y axis.
/// It exposes a method to allow other behaviors to rotate it, and has a
/// "correct" position.
/// </summary>
public class Ring : MonoBehaviour
{
    /// <summary>
    /// Invoked when the ring is rotated.
    /// </summary>
    public event Action RingStateChanged;

    [SerializeField]
    private RingPuzzle puzzle;

    [SerializeField]
    [Tooltip("The correct rotation in degrees for this object. So far this only works for the Y axis.")]
    [Range(0, 360)]
    private float correctRotation = 0;

    // This is the current Y axis rotation of the object.
    private float currentRotation;

    private MeshRenderer meshRenderer;
    private Material nonEmissiveMaterial;
    [SerializeField]
    [Tooltip("The emissive material replacement for when the dial is in the correct spot.")]
    private Material emissiveMaterial;

    /// <summary>
    /// Is the object in the correct rotational position?
    /// </summary>
    public bool IsRotationCorrect { get; private set; }

    // When the puzzle has been solved, we don't want the dials to rotate anymore.
    private bool canRotate = true;

    private void OnEnable()
    {
        if (puzzle != null) puzzle.PuzzleUnlocked += DisableRotation;
    }
    private void OnDisable()
    {
        if (puzzle != null) puzzle.PuzzleUnlocked -= DisableRotation;
    }

    /// <summary>
    /// Initialize needed values and disable the light.
    /// </summary>
    private void Start ()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if(meshRenderer != null)
        {
            nonEmissiveMaterial = meshRenderer.material;

        }
        // Set the current rotation to its initial value
        currentRotation = this.transform.localRotation.y;
        IsRotationCorrect = false;
	}

    /// <summary>
    /// Rotates the object by the amount specified in degrees.
    /// </summary>
    /// <param name="amountToRotate">The amount in degrees</param>
    public void Rotate(Vector3 amountToRotate)
    {
        if(canRotate)
        {
            // This script currently only cares about the y rotation of the object.
            currentRotation += amountToRotate.x;
            // If the object has gone full circle or beyond, make sure to reset it.
            if (currentRotation > 360)
            {
                currentRotation -= 360;
            }
            this.transform.Rotate(amountToRotate);

            // Check the state of the current rotation to see if the ring is in the correct position.
            CheckCurrentRotation();
            // Notify subscribers that the ring was rotated.
            RingStateChanged.Invoke();
        }
    }

    /// <summary>
    /// Check to see if the rings is in the correct position or not.
    /// </summary>
    private void CheckCurrentRotation()
    {
        // If the object is in the correct place...
        if (currentRotation == correctRotation)
        {
            // ... turn on the emissive and change the boolean accordingly.
            IsRotationCorrect = true;
            if(meshRenderer != null)
            {
                meshRenderer.material = emissiveMaterial;
            }
        }
        // If not...
        else
        {
            // ... turn on the emissive and change the boolean accordingly 
            IsRotationCorrect = false;
            if(meshRenderer != null)
            {
                meshRenderer.material = nonEmissiveMaterial;
            }
        }
    }

    /// <summary>
    /// Disables rotation of the dial/ring.
    /// </summary>
    private void DisableRotation()
    {
        canRotate = false;
    }
}
