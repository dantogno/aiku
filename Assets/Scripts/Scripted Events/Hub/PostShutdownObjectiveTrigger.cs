using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class changes the player's objective text when they leave the engine control room
/// after the generator shutdown event.
/// The script is applied to the night vision trigger zone right outside the engine control room.
/// </summary>

public class PostShutdownObjectiveTrigger : MonoBehaviour
{
    [SerializeField, Tooltip("This is the component that writes text to the player's screen.")]
    private TextWriter textWriter;

    [SerializeField, Tooltip("The player's new objective")]
    private string[] objective;

    private bool generatorExploded = false, hasChangedText = false;

    private void OnEnable()
    {
        EngineSequenceManager.OnShutdown += OnGeneratorExploded;
    }
    private void OnDisable()
    {
        EngineSequenceManager.OnShutdown -= OnGeneratorExploded;
    }

    private void OnTriggerEnter(Collider other)
    {
        // The objective text can change if the player enters the trigger, the generator has exploded, and the text hasn't already changed.
        bool canChangeText = other.tag == "Player" && generatorExploded && !hasChangedText;

        if (canChangeText) ChangeText();
    }

    /// <summary>
    /// When the generator explodes, the player's objective can change.
    /// </summary>
    private void OnGeneratorExploded()
    {
        generatorExploded = true;
    }

    /// <summary>
    /// Print the player's new objective to the HUD canvas.
    /// </summary>
    private void ChangeText()
    {
        textWriter.DisplayText(objective);

        hasChangedText = true;
    }
}
