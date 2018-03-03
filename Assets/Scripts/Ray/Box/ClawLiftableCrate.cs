using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawLiftableCrate: MonoBehaviour {

    /// <summary>
    ///  Class for any object that the claw sould be able to pick up
    /// </summary>
    /// 
    
     // will this object move with the claw.
    private bool isUsingParentsTransform;

    public void Start()
    {
        isUsingParentsTransform = false;
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void Update()
    {
       if (isUsingParentsTransform)
        {
            this.transform.position = new Vector3( this.transform.parent.position.x, this.transform.parent.position.y - this.gameObject.GetComponent<Collider>().bounds.size.y, this.transform.parent.position.z);
        }
    }

    // When we drop the object, we will no longer use the parent's transform
    // we will also want gravity to effect the liftable object
    public void DropObject()
    {
        isUsingParentsTransform = false;
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
    // When we lift the object, we will want the object to use the 
    //transform of the parent object, we also would like our phyics not
    // to influence the lifted object.
    public void LiftObject()
    {
        isUsingParentsTransform = true;
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
}
