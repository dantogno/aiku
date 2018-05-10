using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabDoor : Door
{
    private bool doneWithTrevorLevel;

    private void OnEnable()
    {
        HubSceneChanger.FinishedLevel += OnFinishedLevel;
    }
    private void OnDisable()
    {
        HubSceneChanger.FinishedLevel -= OnFinishedLevel;
    }

    protected override void OnTriggerStay(Collider other)
    {
        if (doneWithTrevorLevel)
        {
            //hack for solving scanner issue
            if (other.isTrigger)
                return;

            // If the door is powered, closed, unlocked, and the player is close enough, it can open.
            bool canOpen = IsFullyPowered && !open && !locked;

            if (canOpen) OpenDoor();
        }
        else
        {
            base.OnTriggerStay(other);
        }
        
    }
    
    private void OnFinishedLevel(HubSceneChanger.CrewmemberName crewmember)
    {
        if (crewmember == HubSceneChanger.CrewmemberName.Trevor)
            doneWithTrevorLevel = true;
    }
}
