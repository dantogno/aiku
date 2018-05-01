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

    
    private Rigidbody thisObjectsRigidbody;

    private void Start()
    {
        thisObjectsRigidbody = this.GetComponent<Rigidbody>();
    }
    void Update ()
    {
        CheckForGravityChange();
	}

    private void CheckForGravityChange()
    {
        if (Physics.gravity == Vector3.zero)
        {
            thisObjectsRigidbody.isKinematic = true;
        }

        else
        {
            thisObjectsRigidbody.isKinematic = false;
        }
    }
}
