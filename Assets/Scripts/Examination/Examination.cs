using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script allows the Player to examine objects 
/// Attach this script to the objects that you want to examine
/// There are custom settings on thesd Inspector that allows for each object to have their own unique settins if needed
/// Rotation and zoom is on by default, but feel free to disable them
/// NOTE: This focuses on the front of the gameobject, meaning that the forward face of the object is the one that 
/// the camera will zoom focus on upon activating.
/// </summary>

public class Examination : MonoBehaviour, IInteractable
{
    #region  Private Variables 
    private Camera mainCamera;
    //Sets the original position of the obect upon start.
    private Vector3 StartPos;
    //Sets the original rotation of the object upon start.
    private Quaternion originalRotationValue;
    //Distance is what we set for the zoom function to work.
    private float addedDistance;
    //Check to see if the player is currently inspecting something. 
    private bool isInspecting;
    private float rotX;
    private float rotY;
    CustomRigidbodyFPSController playerController;
    private GameObject Player;

    #endregion
    /// <summary>
    /// These are the serialized fields that can be modified in the inspector
    /// Made simple enough so anyone can edit based on the height of their object
    /// </summary>
    #region Inspector Elements
    [Header("How far do you want the object from the camera")]
    [Tooltip("Choose from 0 units to 2 units")]
    [Range(0.0f, 2.0f)]
    [SerializeField]
    float distance = .83f;

    [Header("Animation Speed")]
    [Tooltip("Higher the Value, Quicker the animation. Recommend higher number for pickup")]
    [Range(0.0f, 10.0f)]
    [SerializeField]
    int smooth = 5;

    [Header("Customize Limitations")]
    [Space(10)]
    [Tooltip("Allow the object to be manipulated")]
    [SerializeField]
    bool isRotationAllowed = true;
    [Tooltip("Sensitivity for the rotation")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    float mouseSensitivity = 0.5f;

    [Space(10)]
    [Tooltip("Allow the object to zoom in towards the camera or not")]
    [SerializeField]
    bool isZoomEnabled = true;
    [Tooltip("Sensitivity for the zomm effect. Note: negative values inverse the control of how you zoom in")]
    [SerializeField]
    float scrollSensitivity = -2.0f;

    [Space(10)]
    [Tooltip("Minimum X Rotation value for object")]
    [SerializeField]
    float MIN_X = -45f;

    [Tooltip("Maximum X Rotation value for object")]
    [SerializeField]
    float MAX_X = 45;

    [Tooltip("Minimum Y Rotation value for object")]
    [SerializeField]
    float MIN_Y = -45;

    [Tooltip("Maximum Y Rotation value for object")]
    [SerializeField]
    float MAX_Y = 45;

    [Space(10)]
    [Tooltip("Minimum Zoom Distance Value for object")]
    [Range(-1.0f, 0.0f)]
    [SerializeField]
    float ZoomMin = -.2f;

    [Tooltip("Maximum Zoom Distance Value for object")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    float ZoomMax = .2f;
    #endregion

    /// <summary>
    /// Sets the starting position and rotational value of the object upon start.
    /// </summary>
    void Start()
    {
        Player = null;
        playerController = null;
        mainCamera = null;
        isInspecting = false;
        StartPos = gameObject.transform.position;
        originalRotationValue = gameObject.transform.rotation;

    }

    public void Interact(GameObject agentInteracting)
    {
        if (isInspecting)
        {
            //Resets the player to Null after the leaving the inspection 
            Player = null;
            FinishInspect();
        }
        else
        {
            //Sets the player to the agentInteracting.
            Player = agentInteracting;
            //Calls for the player Controller script from the Player. 
            playerController = Player.GetComponent<CustomRigidbodyFPSController>();
            //Calls for the camera component from the player.
            mainCamera = Player.GetComponentInChildren<Camera>();
            //Set to true to enable the inspection sequence. 
            isInspecting = true;
        }

    }
    void Inspecting()
    {
        //Lerps the gameobject to the main camera.
        transform.position = Vector3.Lerp(transform.position, mainCamera.transform.position + mainCamera.transform.forward * (distance + addedDistance), Time.deltaTime * smooth);
        //Since we do not want to move with the object, we disable the script.
        playerController.enabled = false;
        //We set this to a trigger to ignore collision of the gameobject.
        GetComponent<Collider>().isTrigger = true;
        //This is how we get the gameobject to positon itself to face the camera. This position the front of the gameobject to the camera (Z axis I believe in Maya)
        transform.LookAt(mainCamera.transform);

        //In the case that the object contains a rigidbody, set this in motion. It disables gravity, collision and kinematic to prevent it from sliding, floating and colliding. Essentially, treat it as if it was not a rigidbody for the moment.
        if (GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().detectCollisions = true;
            GetComponent<Rigidbody>().isKinematic = false;
        }
        //If rotation is enabled, call for the function of Rotation.
        if (isRotationAllowed == true)
        {
            Rotation();
        }
        //If zoom is enabled, set a value for Added Distance. I added clamps to be set by the user. 
        if (isZoomEnabled == true)
        {
            addedDistance += Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;
            addedDistance = Mathf.Clamp(addedDistance, ZoomMin, ZoomMax);

        }

    }
    void FixedUpdate()
    {
        if (isInspecting)
        {
            Inspecting();
        }
    }
    ///<summary>
    ///This is how we finish the inspecting of the object
    ///It should be compatible with both Rigidbody and objects without Rigidbodies 
    ///</summary>
    public void FinishInspect()
    {
        isInspecting = false;
        //Disable Trigger to make it a collidable object. 
        GetComponent<Collider>().isTrigger = false;
        addedDistance = 0;
        //Enable back the player controller. Needed to continue
        playerController.enabled = true;
        //Sets the gameobject to go back to the original positon and rotation.
        gameObject.transform.position = StartPos;
        gameObject.transform.rotation = originalRotationValue;

        if (GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    //In the case that rotation is called, this is what gets the ball rolling 
    private void Rotation()
    {
        rotX += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotY += Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotX = Mathf.Clamp(rotX, MIN_X, MAX_X);
        rotY = Mathf.Clamp(rotY, MIN_Y, MAX_Y);
        this.transform.Rotate(-rotY, -rotX, 0);
    }
}