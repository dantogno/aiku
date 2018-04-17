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
    /// This event is called after the player chooses a survivor.
    /// It is invoked with the name of the survivor that was chosen.
    /// </summary>
    public event Action<string> ChoseACrewmember;

    // A variable to keep track of the current state of interaction.
    // It dictates what happens when player interacts with the screen.
    private bool readyToChooseSurvivors = false;

    private void OnEnable()
    {
        // When the player has finished playing the levels, it changes the interaction
        // functionality of this script.
        EndingScreen.DoneWithLevels += SwitchInteractionModeToFinalChoice;
    }
    private void OnDisable()
    {
        EndingScreen.DoneWithLevels -= SwitchInteractionModeToFinalChoice;
    }

    /// <summary>
    /// Transports the player to another level. If all of the levels have been
    /// played, the player instead chooses a survivor.
    /// </summary>
    /// <param name="agentInteracting">The agent interacting with this object</param>
    public override void Interact(GameObject agentInteracting)
    {
        if (agentInteracting.GetComponent<PowerableObject>() != null)
        {
            // Remove all power from the agent if it is a powerable object.
            agentInteracting.GetComponent<PowerableObject>().PowerOff();
        }
        if (!readyToChooseSurvivors)
        {
            // Default interaction switches levels.
            base.Interact(agentInteracting);
        }
        else if (readyToChooseSurvivors)
        {
            // After interaction is switched, interacting with the screens
            // should indicate which crewmember the player has chosen, instead
            // of transporting them to another level.
            ChooseCrewmember();
        }
    }

    /// <summary>
    /// After the player has played through all the levels,
    /// their final act is to pick the two crewmembers who will live.
    /// </summary>
    private void SwitchInteractionModeToFinalChoice()
    {
        readyToChooseSurvivors = true;

        // We don't want the vfx getting in the way of this important philosophical moment.
        GlitchValueGenerator glitchValueGenerator = GetComponentInParent<GlitchValueGenerator>();
        if (glitchValueGenerator != null)
            glitchValueGenerator.enabled = false;
    }

    /// <summary>
    /// The name of the scene tells GameManager.cs which crewmember the player has chosen.
    /// </summary>
    private void ChooseCrewmember()
    {
        // For the purposes of this script, the build index of the scenes
        // need to be constant.
        // We feel like this is more reliable than relying on the scene names, since
        // they might change.
        int buildIndex = SceneManager.GetSceneByName(sceneToLoad).buildIndex;

        switch (buildIndex)
        {
            // Norma's build index = 2
            case 2:
                ChoseACrewmember.Invoke("Norma");
                break;
            // Trevor's build index = 3
            case 3:
                ChoseACrewmember.Invoke("Trevor");
                break;
            // Ray's build index = 4
            case 4:
                ChoseACrewmember.Invoke("Ray");
                break;
            default:
                break;
        }
    }
}
