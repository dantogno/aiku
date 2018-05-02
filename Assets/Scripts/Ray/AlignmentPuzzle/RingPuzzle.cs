using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class goes on any object in the Alignment Puzzle prefab.
/// It holds a reference to the rings that are a part of the puzzle.
/// Its purpose is to broadcast an event when the puzzle has been completed.
/// </summary>
public class RingPuzzle : MonoBehaviour
{
    /// <summary>
    /// Invoked when the puzzle is solved.
    /// </summary>
    public event Action PuzzleUnlocked;
    /// <summary>
    /// Invoked when the puzzle is unsolved.
    /// </summary>
    public event Action PuzzleLocked;

    [SerializeField]
    [Tooltip("When all of these rings are in the correct position, the puzzle is completed.")]
    private Ring[] ringsToUnlock;

    // Used to determine whether all of the rings are in the correct position.
    private bool puzzleComplete = false;
    // We only want to sent the event once.
    private bool hasSentLockedEvent = false;

    /// <summary>
    /// Subscribe to each ring's state changed event.
    /// </summary>
    private void OnEnable()
    {
        foreach(Ring ring in ringsToUnlock)
        {
            ring.RingStateChanged += OnRingStateChanged;
        }
    }
    /// <summary>
    /// Unsubscribe from each ring's state changed event.
    /// </summary>
    private void OnDisable()
    {
        foreach(Ring ring in ringsToUnlock)
        {
            ring.RingStateChanged -= OnRingStateChanged;
        }
    }

    /// <summary>
    /// Notify subscribers if the puzzle has been solved or if it was just
    /// unsolved.
    /// </summary>
    private void OnRingStateChanged()
    {
        puzzleComplete = CheckIfShouldGrantAccess();
        // Only sent the moment the puzzle has been completed.
        if (puzzleComplete)
        {
            if(PuzzleUnlocked!= null)
            {
                PuzzleUnlocked.Invoke();
            }
            hasSentLockedEvent = false;
        }
        // Only sent the moment the puzzle stops being completed.
        else if (hasSentLockedEvent == false)
        {
            if(PuzzleLocked != null)
            {
                PuzzleLocked.Invoke();
            }
            hasSentLockedEvent = true;
        }
    }

    /// <summary>
    /// If all of the rings are in the correct position, the puzzle is completed.
    /// If even one ring is in the wrong position, the puzzle is not complete.
    /// </summary>
    /// <returns></returns>
    private bool CheckIfShouldGrantAccess()
    {
        foreach (Ring ring in ringsToUnlock)
        {
            if (ring.IsRotationCorrect == false)
            {
                return false;
            }
        }
        return true;
    }
}
