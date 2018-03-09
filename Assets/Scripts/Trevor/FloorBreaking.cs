using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBreaking : MonoBehaviour
{
    Rigidbody thisRigidbody;

	// Use this for initialization
	private void Start ()
    {
        thisRigidbody = gameObject.GetComponent<Rigidbody>();
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
            if (thisRigidbody != null)
            {
                thisRigidbody.isKinematic = false;
                thisRigidbody.useGravity = true;
            }
        }
    }
}
