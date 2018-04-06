using UnityEngine;
using System.Collections;

public class RotateScript : MonoBehaviour {

    /// <summary>
    /// This script is meant for constantly rotating gears, and constantly rotates the object it is attached to.
    /// </summary>

    #region Rotation Variables 
    [Tooltip("A vector that holds the x,y,z values for rotation speed")]
    [SerializeField]
    public Vector3 RotationSpeed;

    [Tooltip("Determines if object can rotate at all")]
    [SerializeField]
    public bool CanRotate;

    [Tooltip("Determines if object can rotate around x axis")]
    [SerializeField]
    public bool CanRotateX;

    [Tooltip("Determines if object can rotate around y axis")]
    [SerializeField]
    public bool CanRotateY;

    [Tooltip("Determines if object can rotate around z axis")]
    [SerializeField]
    public bool CanRotateZ;
   
    //Space used when calling transform.Rotate
    private Space rotationSpace = Space.Self;
    #endregion

    // Update rotates the object, and if x,y,z are not rotatable, then it resets the respective variable to 0
    void Update () {

        if (CanRotate)
        {
            Rotation();
        }

        // Resets the corresponding variable to 0 if !CanRotate
        if (!CanRotateX)
        {
            RotationSpeed.x = 0f;
        }
        if (!CanRotateY)
        {
            RotationSpeed.y = 0f;
        }
        if (!CanRotateZ)
        {
            RotationSpeed.z = 0f;
        }
    
    }

    //Rotates the object
    private void Rotation()
    {
        this.transform.Rotate(RotationSpeed.x * Time.deltaTime, RotationSpeed.y * Time.deltaTime, RotationSpeed.z * Time.deltaTime, rotationSpace);
    }
}
