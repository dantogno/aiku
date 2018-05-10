using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is a specialized version of the Scene Changer intended for the Hub level.
/// It should go on the cryochambers that the player can interact with.
/// When the player has played through all of the levels, it switches the functionality
/// of the interaction from entering the levels to choosing survivors.
/// </summary>
public class HubSceneChanger : SceneChanger
{
    /// <summary>
    /// This event is called after the player finishes playing through a level.
    /// It is invoked with the name of the crewmember whose level was finished.
    /// </summary>
    public static event Action<CrewmemberName> FinishedLevel;

    public enum CrewmemberName { Norma, Trevor, Ray }

    [SerializeField, Tooltip("The name of the crewmember, since it's harder to slip up with enums than strings.")]
    private CrewmemberName crewmemberName;

    [SerializeField, Tooltip("The cryochamber associated with this scene changer.")]
    private Cryochamber cryochamber;

    private void OnEnable()
    {
        // When the player finishes a level, find out which one and broadcast appropriate event.
        // We are not unsubscribing from this event, just to make sure it's not accidentally skipped.
        SceneTransition.SceneChangeFinished += CheckWhichLevelWasFinished;
    }

    /// <summary>
    /// When a level is exited, check which one it is and broadcast the corresponding event.
    /// </summary>
    private void CheckWhichLevelWasFinished()
    {
        foreach (string s in SceneTransition.LoadedSceneNames)
        {
            if (s == sceneToLoad)
            {
                FinishedLevel.Invoke(crewmemberName);
            }
        }
    }

    public override void Interact(GameObject agentInteracting)
    {
        base.Interact(agentInteracting);

        cryochamber.DisableSceneTransitionMonitor();
    }
}
