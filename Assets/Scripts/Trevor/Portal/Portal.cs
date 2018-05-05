using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IInteractable 
{
    [SerializeField] Portal PortalBuddy;                    //Portal to which interacting GameObject is sent
    [SerializeField] Vector3 DefaultPlacement;              //Default location to place the teleported GameObject
    [SerializeField] bool AlwaysUseDefaultPlacement;        //When a GameObject is teleported to this Portal, it will always be placed at the Default Placement value
    [SerializeField] bool UponArrivalLookAtPortal;          //When GameObject is teleported to Portal Buddy, does it "look" at Portal Buddy

    void IInteractable.Interact(GameObject agent)
    {
        //Debug.Log("Portal activated by " + agent);
        Debug.Log(agent.name);
        Teleport(agent);
    }

    // Use this for initialization
    void Start ()
    {
        
	}

    void Awake ()
    {
        if (PortalBuddy == null)
        {
            Debug.LogError("All Portals must have a \"Portal Buddy\" to send the interacting GameObject to.");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Teleport(GameObject agent)
    {
        SavePlayerLocation(agent);

        if (PortalBuddy.savedPlyrLoc == Vector3.zero || AlwaysUseDefaultPlacement)
        {
            agent.transform.position = PortalBuddy.DefaultPlacement;

            if (UponArrivalLookAtPortal)
            {
                Debug.LogError("Unimplemented");
            }

            Debug.Log(agent + " teleported to " + PortalBuddy.DefaultPlacement);
        }
        else
        {
            agent.transform.position = PortalBuddy.savedPlyrLoc;
            agent.transform.rotation = Quaternion.Euler(PortalBuddy.savedPlyrRot);

            Debug.Log(agent + " teleported to " + PortalBuddy.savedPlyrLoc + " " + PortalBuddy.savedPlyrRot);
        }
    }

    Vector3 savedPlyrLoc;
    Vector3 savedPlyrRot;

    void SavePlayerLocation(GameObject plyr)
    {
        this.savedPlyrLoc = plyr.transform.position;
        this.savedPlyrRot = plyr.transform.rotation.eulerAngles;

        Debug.Log(plyr + " location saved " + savedPlyrLoc + " " + savedPlyrRot);
    }
}
