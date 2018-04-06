using System.Collections.Generic;
using UnityEngine;

public class ScanInteractable : MonoBehaviour
{
    //Max distance of the scan
    [Tooltip("Determines x of scan")]
    [SerializeField]
    [Range(1.0f, 20.0f)]
    private float sizeX = 10.0f;
    [Tooltip("Determines y of scan")]
    [SerializeField]
    [Range(1.0f, 10.0f)]
    private float sizeY = 4.0f;
    [Tooltip("Determines z of scan")]
    [SerializeField]
    [Range(1.0f, 20.0f)]
    private float sizeZ = 10.0f;

    private BoxCollider scanTrigger;
    private Transform cameraLoc;
    private Transform playerLoc;

    private List<GameObject> powerSwitches;

    private void Awake()
    {
        scanTrigger = this.GetComponent<BoxCollider>();
        scanTrigger.size = new Vector3(sizeX,sizeY,sizeZ);
        cameraLoc = Camera.main.transform;
        powerSwitches = new List<GameObject>();
        playerLoc = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PowerSwitch>() != null)
        {
            powerSwitches.Add(other.gameObject);
        }
    }
    
    private void Update()
    {
        foreach (GameObject ps in powerSwitches)
        {
            if (IsBlocked(ps))
                EdgeInstance.DisableShader(ps);
            else
                EdgeInstance.TurnOnShader(ps);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (powerSwitches.Remove(other.gameObject))
        {
            EdgeInstance.DisableShader(other.gameObject);
            EdgeInstance.DestroyShader(other.gameObject);
        }
    }

    private bool IsBlocked(GameObject toCheck)
    {
        bool toReturn;
        Vector3 dirFromPlayer = toCheck.transform.position - cameraLoc.position;
        RaycastHit hit; //holder for the collided object
        Physics.Raycast(cameraLoc.position, dirFromPlayer, out hit); // begin raycast from the player camera center
        //Hit.collider is null when a hit object has the same local position as the gameobject this script is attached to.
        //This causes a null reference exception. (Might happen in some other cases too).
        if (toCheck == hit.collider.gameObject) // check if it hit the object to check
            toReturn = false; // not blocked
        else
            toReturn = true;
        return toReturn; //its blocked
    }
}