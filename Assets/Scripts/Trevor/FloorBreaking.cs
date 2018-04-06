using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Script manages when the Player walks over the breakable floor panels and enables
/// the physics of said floor panels. 
/// </summary>
public class FloorBreaking : MonoBehaviour
{
    private Rigidbody[] thisRigidbody;

    private AudioSource breakingFloorAudio;
    private bool hasPlayedAudio = false;

	// Use this for initialization
	private void Start ()
    {
        breakingFloorAudio = GetComponent<AudioSource>();
        thisRigidbody = gameObject.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody fTile in thisRigidbody)
        {
            fTile.isKinematic = true;
            fTile.useGravity = false;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        PlayerStepsOnBreakableFloor(other);
    }

    /// <summary>
    /// Activates gravity/physics on the object if the player collides with it.
    /// </summary>
    /// <param name="other"></param>
    private void PlayerStepsOnBreakableFloor(Collider plyrCol)
    {
        if (plyrCol.gameObject.tag == "Player")
        {
            if(!hasPlayedAudio)
            {
                breakingFloorAudio.Play();
                hasPlayedAudio = true;
            }
            //Debug.Log("Derrrr");
            if (thisRigidbody != null)
            {
                foreach (Rigidbody fTile in thisRigidbody)
                {
                    fTile.isKinematic = false;
                    fTile.useGravity = true;
                }
            }
        }
    }
}
