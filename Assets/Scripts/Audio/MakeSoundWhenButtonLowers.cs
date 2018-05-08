using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class plays a sound when the button lowers after the player completes the hologenerator puzzle.
/// The script is applied to a prefab which should be added into the hologenerator asset's hierarchy.
/// </summary>

public class MakeSoundWhenButtonLowers : MonoBehaviour
{
    [SerializeField, Tooltip("The hub's ring puzzle on the hologenerator.")]
    private RingPuzzle puzzle;

    [SerializeField, Tooltip("The AudioSource component which plays when the puzzle is complete.")]
    private AudioSource puzzleSource;

    private void OnEnable()
    {
        puzzle.PuzzleUnlocked += OnPuzzleUnlocked;
    }
    private void OnDisable()
    {
        puzzle.PuzzleUnlocked -= OnPuzzleUnlocked;
    }

    /// <summary>
    /// Play a sound when the player completes the puzzle.
    /// </summary>
    private void OnPuzzleUnlocked()
    {
        puzzleSource.Play();
    }
}
