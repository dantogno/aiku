using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class toggles the emissive map on the fuel station bot face material.
/// The script is applied to the fuel station in the engine room.
/// </summary>

public class ToggleFuelBotFace : MonoBehaviour
{
    [SerializeField, Tooltip("This material's emissive material should be toggled when tasks are completed.")]
    private Renderer botFaceRenderer;

    [SerializeField, Tooltip("This task controls when the bot face's material should be toggled.")]
    private Task fuelErrorTask, shutdownTask;

    private Color originalColor;

    private void OnEnable()
    {
        fuelErrorTask.OnTaskCompleted += TurnOnBotFace;
        shutdownTask.OnTaskCompleted += TurnOffBotFace;
    }
    private void OnDisable()
    {
        fuelErrorTask.OnTaskCompleted -= TurnOnBotFace;
        shutdownTask.OnTaskCompleted -= TurnOffBotFace;
    }

    private void Start()
    {
        originalColor = botFaceRenderer.material.GetColor("_EmissionColor");
        
        TurnOffBotFace();
    }

    /// <summary>
    /// The bot face turns on when the fuel station throws an error.
    /// </summary>
    private void TurnOnBotFace()
    {
        botFaceRenderer.material.SetColor("_EmissionColor", originalColor);
    }

    /// <summary>
    /// The bot face turns off when the generator shuts down.
    /// </summary>
    private void TurnOffBotFace()
    {
        botFaceRenderer.material.SetColor("_EmissionColor", Color.black);
    }
}
