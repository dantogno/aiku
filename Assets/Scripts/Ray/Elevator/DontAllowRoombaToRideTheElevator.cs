using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Apply this to the up elevator button.
/// </summary>

public class DontAllowRoombaToRideTheElevator : MonoBehaviour
{
    private RayElevatorButtonScript upButton;

    private void Awake()
    {
        upButton = GetComponent<RayElevatorButtonScript>();
        upButton.enabled = false;
    }

    private void OnEnable()
    {
        RayDoorTrigger.PlayerEnteredTrigger += ActivateButton;
    }
    private void OnDisable()
    {
        RayDoorTrigger.PlayerEnteredTrigger -= ActivateButton;
    }

    private void ActivateButton()
    {
        upButton.enabled = true;
    }
}
