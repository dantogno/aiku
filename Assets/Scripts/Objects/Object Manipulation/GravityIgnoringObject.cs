using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  A simple script to attach to any object you would not want to move due to
///  initial zero gravity of a scene swap. Attach this class to objects you want to stay in
///  place for the zero gravity, but would want to move later ( example, the boxes in the ray level are stacked in a specific manner that
///  may become disrupted).
/// </summary>

[RequireComponent(typeof(Rigidbody))]

public class GravityIgnoringObject : MonoBehaviour
{
    // This Objects Rigidbody.
    private Rigidbody thisObjectsRigidbody;

    private void Start()
    {
        thisObjectsRigidbody = this.GetComponent<Rigidbody>();
        thisObjectsRigidbody.isKinematic = true;
        SceneTransition.SceneChangeFinished += AllowGravityToMoveObject;
    }

    // Allows gravity and all other forces to move this object. 
    private void AllowGravityToMoveObject()
    {
        if (Physics.gravity != Vector3.zero && this.gameObject.GetComponent<Rigidbody>() != null)
        {
            thisObjectsRigidbody.isKinematic = false;
        }
    }
}
