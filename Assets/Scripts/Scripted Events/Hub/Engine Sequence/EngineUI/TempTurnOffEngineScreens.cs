using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this turns off the screens in the absence of actual powerable functionality
/// </summary>
public class TempTurnOffEngineScreens : MonoBehaviour {

    private InteractableEngineScreen[] screens;

    [SerializeField]
    private Task turnOffPowerTask;

	// Use this for initialization
	private void Start () {
        screens = FindObjectsOfType<InteractableEngineScreen>();
        turnOffPowerTask.OnTaskCompleted += TurnOffAllEngineScreens;
	}
	
	private void TurnOffAllEngineScreens()
    {
        foreach(InteractableEngineScreen screen in screens)
        {
            screen.PowerOff();
            screen.GetComponent<Collider>().enabled = false;    // DW

            foreach (AudioSource a in screen.GetComponentsInChildren<AudioSource>())
            {
                a.volume = 0;
            }
        }
    }
}
