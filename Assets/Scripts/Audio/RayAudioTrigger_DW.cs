using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayAudioTrigger_DW : MonoBehaviour
{
    [SerializeField, Tooltip("The GameObject with the first voice line that should play.")]
    private GameObject voiceObject;

    [SerializeField, Tooltip("How long to wait until playing the voice line.")]
    private float waitTime = 1;

    private bool hasPlayedVoiceLine = false;

    private void OnEnable()
    {
        RoombaConsole.SwitchedControllers += CheckIfCanActivateVoiceObject;
    }
    private void OnDisable()
    {
        RoombaConsole.SwitchedControllers -= CheckIfCanActivateVoiceObject;
    }

    /// <summary>
    /// When the player switches from the roomba to Ray for the first time,
    /// activate the GameObject with the audio functionality for the first voice line.
    /// </summary>
    private void CheckIfCanActivateVoiceObject()
    {
        if (!hasPlayedVoiceLine)
        {
            Invoke("ActivateVoiceObject", waitTime);
        }
    }


    private void ActivateVoiceObject()
    {
        voiceObject.SetActive(true);
        hasPlayedVoiceLine = true;
    }
}
