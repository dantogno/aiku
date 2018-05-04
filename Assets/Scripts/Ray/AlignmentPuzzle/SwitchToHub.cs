using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class should go on any part of the ring alignment puzzle
/// in the Ray level.
/// Its purpose is to switch to the Hub scene once the button has been pressed.
/// </summary>
public class SwitchToHub : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The button that should initiate the scene change.")]
    private RingPuzzleButton button;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        audioSource.Play();

        if (!audioSource.isPlaying)
        {
            button.ButtonPressed += SwitchToHubScene;
        }
    }
    private void OnDisable()
    {
        button.ButtonPressed -= SwitchToHubScene;
    }

    /// <summary>
    /// Loads the Hub level.
    /// </summary>
    private void SwitchToHubScene()
    {
        SceneTransition.LoadScene("Hub");
    }
}
