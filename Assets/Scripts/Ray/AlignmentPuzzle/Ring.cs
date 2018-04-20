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
    [Tooltip("A light that turns on when the object in in the correct position.")]
    private Light correspondingLight;
    [SerializeField]
    [Tooltip("The correct rotation in degrees for this object. So far this only works for the Y axis.")]
    [Range(0, 360)]
    private float correctRotation = 0;

    // This is the current Y axis rotation of the object.
    private float currentRotation;

    /// <summary>
    /// Is the object in the correct rotational position?
    /// </summary>
    public bool IsRotationCorrect { get; private set; }

	/// <summary>
    /// Initialize needed values and disable the light.
    /// </summary>
	private void Start ()
    {
        // Disable the light that indicates whether the object is in the correct place.
        if(correspondingLight != null)
        {
            correspondingLight.enabled = false;
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
        // This script currently only cares about the y rotation of the object.
        currentRotation += amountToRotate.y;
        // If the object has gone full circle or beyond, make sure to reset it.
        if(currentRotation > 360)
        {
            currentRotation -= 360;
        }
        this.transform.Rotate(amountToRotate);

        // Check the state of the current rotation to see if the ring is in the correct position.
        CheckCurrentRotation();
        // Notify subscribers that the ring was rotated.
        RingStateChanged.Invoke();
    }

    /// <summary>
    /// Check to see if the rings is in the correct position or not.
    /// </summary>
    private void CheckCurrentRotation()
    {
        // If the object is in the correct place...
        if (currentRotation == correctRotation)
        {
            // ... enable the light and change the boolean accordingly.
            IsRotationCorrect = true;
            correspondingLight.enabled = true;
        }
        // If not...
        else
        {
            // ... disable the light and change the boolean accordingly 
            IsRotationCorrect = false;
            correspondingLight.enabled = false;
        }
    }
}
