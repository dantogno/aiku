using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainElevatorButton : MonoBehaviour, IInteractable
{
    [Tooltip("The elevator gate at the top of the elevator.")]
    [SerializeField] GameObject elevatorGate;

    [Tooltip("The time in seconds that the gate will be open before it closes.")]
    [SerializeField] float waitTime = 3;

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
        // HACK. Robert - I may have made a bad decision when I added the argument at the very end of InteractWithSelectedObject.cs.
        // I passed the GameObject's root, rather than the GameObject itself. I feel like I can only explain the consequences of this decision in person, however.
        // For now, suffice to say that this line works, it will likely always work, but it is a hack all the same.
        GameObject.FindGameObjectWithTag("Player").transform.SetParent(null);

        // Deactivate the elevator gate.
        elevatorGate.SetActive(false);

        // Give the player the opportunity to leave.
        yield return new WaitForSeconds(waitTime);

        // Activate the gate again.
        elevatorGate.SetActive(true);
    }
}
