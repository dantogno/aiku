using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubSceneChanger : SceneChanger
{
    public event Action<string> ChoseACrewmember;   // DW: Called after all levels have been completed and player chooses a survivor.

    private bool readyToChooseSurvivors = false;    // DW: Dictates what happens when player interacts with the screen.

    private void OnEnable()
    {
        // DW:
        EndingScreen.DoneWithLevels += SwitchInteractionModeToFinalChoice;
    }
    private void OnDisable()
    {
        // DW:
        EndingScreen.DoneWithLevels -= SwitchInteractionModeToFinalChoice;
    }

    public override void Interact(GameObject agentInteracting)
    {
        // DW
        if (agentInteracting.GetComponent<PowerableObject>() != null)
        {
            agentInteracting.GetComponent<PowerableObject>().PowerOff();
        }
        if (!readyToChooseSurvivors)
        {
            base.Interact(agentInteracting);
        }
        else if (readyToChooseSurvivors)
        {
            ChooseCrewmember();
        }
    }

    /// <summary>
    /// DW:
    /// After the player has played through all the levels,
    /// their final act is to pick the two crewmembers who will live.
    /// </summary>
    private void SwitchInteractionModeToFinalChoice()
    {
        readyToChooseSurvivors = true;

        if (GetComponentInParent<GlitchValueGenerator>() != null)
            GetComponentInParent<GlitchValueGenerator>().enabled = false;   // We don't want the vfx getting in the way of this important philosophical moment.
    }

    /// <summary>
    /// DW:
    /// The name of the scene tells GameManager.cs which crewmember the player has chosen.
    /// </summary>
    private void ChooseCrewmember()
    {
        switch (sceneToLoad)
        {
            case "DanielScene":
                ChoseACrewmember.Invoke("Norma");
                break;
            case "TrevorLevelGDC":
                ChoseACrewmember.Invoke("Trevor");
                break;
            case "RaySceneGDC":
                ChoseACrewmember.Invoke("Ray");
                break;
            default:
                break;
        }
    }
}
