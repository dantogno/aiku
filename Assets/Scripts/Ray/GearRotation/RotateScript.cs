using UnityEngine;
using System.Collections;

/// <summary>
/// This script is meant for constantly rotating gears, and constantly rotates the object it is attached to.
/// </summary>
public class RotateScript : MonoBehaviour
{

    #region Rotation Variables 
    [Tooltip("A vector that holds the x,y,z values for rotation speed")]
    [SerializeField]
    private Vector3 rotationSpeed;

    [Tooltip("Determines if object can rotate at all")]
    [SerializeField]
    public bool CanRotate;
   
    //Space used when calling transform.Rotate
    private Space rotationSpace = Space.Self;
    #endregion

    // Update rotates the object, and if x,y,z are not rotatable, then it resets the respective variable to 0
    void Update ()
    {
        if (CanRotate)
        {
            Rotation();
        }
    }

    //Rotates the object
    private void Rotation()
    {
        this.transform.Rotate(rotationSpeed.x * Time.deltaTime, rotationSpeed.y * Time.deltaTime, rotationSpeed.z * Time.deltaTime, rotationSpace);
    }
}
