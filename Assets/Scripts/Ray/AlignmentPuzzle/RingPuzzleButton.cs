using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script goes on the shutdown button of the Ray puzzle generator.
/// When the player interacts with this button, it invokes an event.
/// </summary>
public class RingPuzzleButton : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Called when the player presses the shutdown button.
    /// </summary>
    public event Action ButtonPressed;

    [SerializeField]
    [Tooltip("The puzzle which enables this button when it is completed.")]
    private RingPuzzle puzzle;

    // The collider on this object.
    private BoxCollider thisCollider;
    // Governs whether the player can interact with the button or not.
    private bool canInteract;

    /// <summary>
    /// Enable interaction when the puzzle is solved and disable it when it isn't
    /// </summary>
    private void OnEnable()
    {
        puzzle.PuzzleUnlocked += EnableInteraction;
        puzzle.PuzzleLocked += DisableInteraction;
    }
    /// <summary>
    /// Unsubscribe from the events when disabled.
    /// </summary>
    private void OnDisable()
    {
        puzzle.PuzzleUnlocked -= EnableInteraction;
        puzzle.PuzzleLocked -= DisableInteraction;
    }

    private void Start()
    {
        // Disable interaction at start.
        thisCollider = GetComponent<BoxCollider>();
        if(thisCollider != null)
        {
            thisCollider.enabled = false;
        }
        canInteract = false;
    }

    /// <summary>
    /// Notify everyone who cares that the button has been pressed.
    /// </summary>
    /// <param name="agentInteracting"></param>
    public void Interact(GameObject agentInteracting)
    {
        if(canInteract)
        {
            // Notify anyone who cares that the button was pressed.
            if (ButtonPressed != null)
            {
                ButtonPressed.Invoke();
            }
        }
    }

    /// <summary>
    /// Enables interaction with this button.
    /// </summary>
    private void EnableInteraction()
    {
        canInteract = true;
        if(thisCollider != null)
        {
            thisCollider.enabled = true;
        }
    }

    /// <summary>
    /// Disables interaction with this button.
    /// </summary>
    private void DisableInteraction()
    {
        canInteract = false;
        if (thisCollider != null)
        {
            thisCollider.enabled = false;
        }
    }
}
