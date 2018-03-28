using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class displays the player's current objective on the screen.
/// </summary>

public class HUDText : MonoBehaviour
{
    #region Text writer component and lines to feed into text writer.

    [Serializable]
    private struct Lines
    {
        public string[] lines;
    }

    [SerializeField]
    private TextWriter textWriter;

    [SerializeField]
    private Lines[] firstGoalLines, secondGoalLines;

    #endregion

    // Tasks which trigger HUD objective text changes.
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
        textWriter.DisplayText(secondGoalLines[0].lines);
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

        textWriter.DisplayText(secondGoalLines[1].lines);
    }

    /// <summary>
    /// Display the text which appears when the player first starts the Hub level.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisplayFirstHubObjective()
    {
        textWriter.DisplayText(firstGoalLines[0].lines);
        yield return StartCoroutine(ClearTextAfterWait(10));
        textWriter.DisplayText(firstGoalLines[1].lines);
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
