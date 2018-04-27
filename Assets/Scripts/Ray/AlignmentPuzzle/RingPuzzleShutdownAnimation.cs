using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script goes on any object in the ringa lignment puzzle.
/// It animates the shutdown door open/closed when the puzzle is
/// completed/uncompleted.
/// </summary>
public class RingPuzzleShutdownAnimation : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The animator for the generator shutdown doors.")]
    private Animator shutdownAnimator;

    /// <summary>
    /// Set the door to closed at the beginning.
    /// </summary>
    private void Start()
    {
        if(shutdownAnimator != null)
        {
            shutdownAnimator.SetBool("isOpen", false);
        }
    }
    /// <summary>
    /// Subscribe to when the puzzle is finished.
    /// </summary>
    private void OnEnable()
    {
        RingPuzzle.PuzzleUnlocked += OpenDoor;
        RingPuzzle.PuzzleUnlocked += CloseDoor;
    }
    private void OnDisable()
    {
        RingPuzzle.PuzzleUnlocked -= OpenDoor;
        RingPuzzle.PuzzleUnlocked -= CloseDoor;
    }

    /// <summary>
    /// Open the shutdown door.
    /// </summary>
    private void OpenDoor()
    {
        if (shutdownAnimator != null)
        {
            shutdownAnimator.SetBool("isOpen", true);
        }
    }
    /// <summary>
    /// Close the shutdown door.
    /// </summary>
    private void CloseDoor()
    {
        if (shutdownAnimator != null)
        {
            shutdownAnimator.SetBool("isOpen", false);
        }
    }
}
