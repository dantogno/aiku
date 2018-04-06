using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for the button at the top of the elevator shaft which opens the gate.
/// The script is applied to the button.
/// </summary>

public class MainElevatorButton : MonoBehaviour, IInteractable
{
    public event Action PressedButton;

    [Tooltip("The elevator gate at the top of the elevator.")]
    [SerializeField] private GameObject elevatorGate;

    [Tooltip("The time in seconds that the gate will be open before it closes.")]
    [SerializeField] private float waitTime = 3;

    [SerializeField, Tooltip("When the player has pressed the button, the SOS task is activated.")]
    private GameObject SOSTaskSequence;

    [SerializeField, Tooltip("The animator component for the gate at the top of the elevator.")]
    private Animator gateAnimator;

    public void Interact(GameObject otherObject)
    {
        StartCoroutine(WaitToCloseGate(otherObject));
    }

    /// <summary>
    /// Give player enough time to leave the elevator before the gate closes behind them.
    /// </summary>
    /// <param name="otherObject"></param>
    /// <returns></returns>
    private IEnumerator WaitToCloseGate(GameObject otherObject)
    {
        // Open the gate.
        gateAnimator.SetTrigger("Open");
        gateAnimator.GetComponent<AudioSource>().Play();

        // Activate the SOS task.
        SOSTaskSequence.SetActive(true);

        if (PressedButton != null) PressedButton.Invoke();

        // HACK. Robert - I may have made a bad decision when I added the argument at the very end of InteractWithSelectedObject.cs.
        // I passed the GameObject's root, rather than the GameObject itself. I feel like I can only explain the consequences of this decision in person, however.
        // For now, suffice to say that this line works, it will likely always work, but it is a hack all the same.
        GameObject.FindGameObjectWithTag("Player").transform.SetParent(null);

        // Deactivate the elevator gate collider.
        elevatorGate.SetActive(false);

        // Give the player the opportunity to leave.
        yield return new WaitForSeconds(waitTime);

        // Activate the gate collider again.
        elevatorGate.SetActive(true);

        // Close the gate.
        gateAnimator.SetTrigger("Close");
        gateAnimator.GetComponent<AudioSource>().Play();
    }
}
