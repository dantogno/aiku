using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is for the hovering instructions text canvas, which has proven popular as a conveyance device.
/// The script is applied to a dedicated Canvas in the hierarchy under "UI" named "Hovering instructions Canvas".
/// There is a sibling GameObject next to it in the hierarchy called "Hovering Instructions Canvas Destinations".
/// This GameObject's children are the empty RectTransforms spread around the ship which should be added to the destinations array.
/// </summary>

public class HoveringInstructionsPlaceholder : MonoBehaviour
{
    [SerializeField, Tooltip("The text object which displays instruction text to the player.")]
    private Text instructions;

    [SerializeField, Tooltip("The tasks which dictate where the canvas travels to. You can find them under TASKS -> OPENING SEQUENCE TASKS.")]
    private Task[] tasks;

    [SerializeField, Tooltip("The locations that the canvas travels as tasks are completed. This GameObject has a sibling with children which should be added to this array.")]
    private Transform[] destinations;

    [SerializeField, Tooltip("The instructions printed to the text component.")]
    private string[] instructionStrings;

    [SerializeField, Tooltip("How quickly the canvas moves from one destination to another.")]
    private float lerpTime = 1.5f;

    // Used to control special behaviors when certain tasks are complete, and to set the instruction text.
    private int counter = 0;

    private void OnEnable()
    {
        foreach (Task t in tasks) t.OnTaskCompleted += GoToNextDestination;
    }
    private void OnDisable()
    {
        foreach (Task t in tasks) t.OnTaskCompleted -= GoToNextDestination;
    }

    /// <summary>
    /// Move the canvas to a new location.
    /// </summary>
    protected virtual void GoToNextDestination()
    {
        counter++;

        // Set the instructions text to the next set of instructions.
        instructions.text = instructionStrings[counter];

        // When the player starts checking valves, the arrow child of the canvas turns sideways.
        // When the player turns three valves, the arrow returns to its normal position.
        if (counter == 3 || counter == 4) GetComponentInChildren<Animator>().SetTrigger("SwitchModes");

        // Move the canvas.
        if (counter < destinations.Length) StartCoroutine(LerpToDestination(destinations[counter]));
    }

    /// <summary>
    /// Move gradually to a new destination (change position and rotation).
    /// </summary>
    /// <param name="destination"></param>
    /// <returns></returns>
    private IEnumerator LerpToDestination(Transform destination)
    {
        Vector3 originalPosition = transform.position, originalEulers = transform.eulerAngles;

        float elapsedTime = 0;
        while (elapsedTime < lerpTime)
        {
            transform.position = Vector3.Lerp(originalPosition, destination.position, elapsedTime / lerpTime);
            transform.eulerAngles = Vector3.Lerp(originalEulers, destination.eulerAngles, elapsedTime / lerpTime);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        transform.position = destination.position;
        transform.eulerAngles = destination.eulerAngles;
    }
}
