using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class turns the hologenerator puzzle holograms off.
/// It is currently applied to the same GameObject as RingPuzzleButton.cs.
/// </summary>

public class ToggleHolograms : MonoBehaviour
{
    [SerializeField, Tooltip("These are the emissive hologram GameObjects that show up when the player is using the hologenerator puzzle.")]
    private GameObject[] holograms;

    [SerializeField, Tooltip("This is the button for shutting off the bots.")]
    private RingPuzzleButton button;

    private void OnEnable()
    {
        button.ButtonPressed += TurnOffHolograms;
    }
    private void OnDisable()
    {
        button.ButtonPressed -= TurnOffHolograms;
    }

    /// <summary>
    /// If the holograms are on, turn them off, and vis versa.
    /// </summary>
    private void TurnOffHolograms()
    {
        foreach (GameObject g in holograms)
            g.SetActive(false);
    }
}
