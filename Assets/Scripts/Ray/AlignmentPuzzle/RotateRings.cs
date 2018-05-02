using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script goes on an object that can be interacted with.
/// When interacted with, it rotates a corresponding Ring object 
/// around the Y axis by a certain amount.
/// </summary>
public class RotateRings : MonoBehaviour, IInteractable
{
    [SerializeField]
    [Tooltip("The Ring objects that should be rotated.")]
    private Ring correspondingRing;

    [SerializeField]
    [Tooltip("The initial position of the Ring.")]
    private float initialRotation = 90f;
    [SerializeField]
    [Tooltip("How many degrees should the Ring rotate with each interaction?")]
    private float rotationAmount = 90f;
    // This vector holds the rotation amount in its Y component.
    private Vector3 amountToRotate;

    [SerializeField]
    [Tooltip("Governs whether the Ring moves instantly, or Lerps to its destination.")]
    private bool instantRotation = false;

    // A variable to hold the current rotation of the object.
    private Vector3 currentRotation;
    // A variable to hold the desired rotation of the object.
    private Vector3 desiredRotation;
    // If the ring is in the process of Lerp rotating, we don't want
    // another interaction to mess with the process.
    private bool isRotating = false;

	// Use this for initialization
	private void Start ()
    {
        amountToRotate = new Vector3(rotationAmount, 0, 0);
        RotateRingToInitialPosition();
	}

    /// <summary>
    /// Rotate the ring to its initial position at the start.
    /// </summary>
    private void RotateRingToInitialPosition()
    {
        // If we're not using Lerp...
        if (instantRotation)
        {
            // ... rotate the ring instantly.
            Vector3 initialAmountToRotate = new Vector3(initialRotation, 0, 0);
            correspondingRing.Rotate(initialAmountToRotate);
        }
        // If we are using Lerp...
        else
        {
            // ...store the variables and take care of rotating in Update.
            currentRotation = correspondingRing.transform.localRotation.eulerAngles;
            desiredRotation = currentRotation + new Vector3(initialRotation, 0, 0);
        }
    }

    /// <summary>
    /// Rotate the ring incrementally if we are using Lerp rotation.
    /// </summary>
    private void Update()
    {
        // Update is only needed when the ring is lerping.
        if(instantRotation == false)
        {
            // For some reason, the lerp speed needs to be 0.5f. Otherwise, the rotation doesn't work as intended.
            // I didn't have the time to figure this out quickly.
            currentRotation = Vector3.Lerp(currentRotation, desiredRotation, 0.5f);
            correspondingRing.Rotate(desiredRotation - currentRotation);
            // Alow interaction again after the ring is in its intended position.
            if (currentRotation == desiredRotation)
            {
                isRotating = false;
            }
        }
    }

    /// <summary>
    /// Rotate the corresponding ring object when this is interacted with.
    /// </summary>
    /// <param name="agentInteracting"></param>
    public virtual void Interact(GameObject agentInteracting)
    {
        //TODO: Add rotating animation for this object

        // If we're not using Lerp...
        if (instantRotation)
        {
            // ... rotate the ring instantly.
            correspondingRing.Rotate(amountToRotate);
        }
        // If we are using Lerp...
        else
        {
            /// ... as long as the ring isn't currently in the process of rotating...
            if (isRotating == false)
            {
                isRotating = true;
                // ...store the variables and take care of rotating in Update.
                desiredRotation += amountToRotate;
            }
        }
    }
}
