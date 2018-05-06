using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script should go on the interactable portion of the Hub's version of
/// the Ray alignment puzzle (the hologram rings).
/// It replaces the classic rotation functionality with a dummy version until
/// the Ray level has been completed.
/// </summary>
public class HubRotateRings : RotateRings
{
    [SerializeField]
    [Tooltip("The rings that should be rotated for the dummy interaction.")]
    private Ring[] dummyRings;

    [SerializeField]
    [Tooltip("The amount each corresponding ring should be rotated.")]
    private float[] dummyRotation;

    // Has the Ray level been completed?
    private bool rayLevelCompleted;

    /// <summary>
    /// Subscribe to which level was finished.
    /// </summary>
    private void OnEnable()
    {
        HubSceneChanger.FinishedLevel += EnablePuzzle;
    }
    /// <summary>
    /// Unsubscribe from which level was finished.
    /// </summary>
    private void OnDestroy()
    {
        HubSceneChanger.FinishedLevel -= EnablePuzzle;
    }

    /// <summary>
    /// Rotate all rings when the player interacts with it. If the player
    /// has finished the level, just rotate the correct ring instead.
    /// </summary>
    /// <param name="agentInteracting"></param>
    public override void Interact(GameObject agentInteracting)
    {
        if (rayLevelCompleted)
        {
            // Defer functionality to the correct interaction.
            base.Interact(agentInteracting);
        }
        else
        {
            // Rotate all of the rings instead of just the correct one.
            RotateAllRings();

            // Spin the hologram ring (not to be confused with the "dial" ring).
            StartCoroutine(RotateHologramRing());
        }
    }

    /// <summary>
    /// Rotate all of the rings instead of just the correct one.
    /// </summary>
    private void RotateAllRings()
    {
        for(int i = 0; i < Mathf.Min(dummyRings.Length, dummyRotation.Length); i++)
        {
            Vector3 rotation = new Vector3(dummyRotation[i], 0, 0);
            dummyRings[i].Rotate(rotation);
        }
    }

    /// <summary>
    /// Enable interaction with the puzzle as intended.
    /// </summary>
    /// <param name="crewmember"></param>
    private void EnablePuzzle(HubSceneChanger.CrewmemberName crewmember)
    {
        if(crewmember == HubSceneChanger.CrewmemberName.Ray)
        {
            rayLevelCompleted = true;
        }
    }
}
