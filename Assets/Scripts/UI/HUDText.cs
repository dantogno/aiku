using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class displays the player's current objective on the screen.
/// It is applied to a dedicated world-space canvas which is a child of the main camera.
/// </summary>

public class HUDText : MonoBehaviour
{
    #region Text writer component and lines to feed into text writer.

    [Serializable]
    private struct Lines
    {
        public string[] lines;
    }

    [SerializeField, Tooltip("The text writer component used to print text lines one letter at a time.")]
    private TextWriter textWriter;

    [Space]

    [SerializeField, Tooltip("The HUD text for the first objective of the game.")]
    private Lines[] firstObjectiveLines;

    [SerializeField, Tooltip("The HUD text for the second objective of the game.")]
    private Lines[] secondObjectiveLines;

    #endregion

    [Header("Tasks Which Trigger HUD Text Changes")]
    [SerializeField]
    private Task firstCheckingTask, elevatorButtonTask, SOSTask;

    private void OnEnable()
    {
        firstCheckingTask.OnTaskCompleted += ClearText;
        elevatorButtonTask.OnTaskCompleted += SetSOSText;
        SOSTask.OnTaskCompleted += SetKeepCrewAliveText;
    }
    private void OnDisable()
    {
        firstCheckingTask.OnTaskCompleted -= ClearText;
        elevatorButtonTask.OnTaskCompleted -= SetSOSText;
        SOSTask.OnTaskCompleted -= SetKeepCrewAliveText;
    }

    private void Start()
    {
        StartCoroutine(DisplayFirstHubObjective());
    }

    /// <summary>
    /// Erase objective text.
    /// </summary>
    private void ClearText()
    {
        StartCoroutine(ClearTextAfterWait(0));
    }

    /// <summary>
    /// When the player emerges from the elevator, their next objective is to activate the SOS beacon.
    /// </summary>
    private void SetSOSText()
    {
        textWriter.DisplayText(secondObjectiveLines[0].lines);
    }

    /// <summary>
    /// When the player activates the SOS beacon, their next objective is to transfer power to the crew life support systems.
    /// </summary>
    private void SetKeepCrewAliveText()
    {
        StartCoroutine(WaitToDisplayKeepCrewAliveText());
    }

    /// <summary>
    /// The text for this objective should appear after a short wait.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitToDisplayKeepCrewAliveText()
    {
        yield return new WaitForSeconds(3);

        textWriter.DisplayText(secondObjectiveLines[1].lines);
    }

    /// <summary>
    /// Display the text which appears when the player first starts the Hub level.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisplayFirstHubObjective()
    {
        textWriter.DisplayText(firstObjectiveLines[0].lines);
        yield return StartCoroutine(ClearTextAfterWait(10));
        textWriter.DisplayText(firstObjectiveLines[1].lines);
    }

    /// <summary>
    /// Clear text after x seconds.
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    private IEnumerator ClearTextAfterWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        textWriter.DisplayText("");
    }
}
