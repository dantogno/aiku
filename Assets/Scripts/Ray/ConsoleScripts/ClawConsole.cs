using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.Characters.FirstPerson;

public class ClawConsole : MonoBehaviour, IInteractable
{
    #region Editor fields
   // [Tooltip("For disabling the fpscontroller while operating the crane")]
    
    [Tooltip("The ClawRigController script attached to the ClawRig Game object")]
    [SerializeField] private ClawRigController clawRigCont;
    [Tooltip("The Transform of the CameraForRigPosition child of this Game Object")]
    //What the player will see after activating the claw console
    [SerializeField] private Transform cameraViewingPosition; 
    [Tooltip("The Speed the camera rotates")]
    [SerializeField] private float rotationSpeed = 20.0f;
    [Tooltip("The speed the camera moves")]
    [SerializeField] private float cameraMoveSpeed = 20.0f;
    #endregion

    #region Private fields
    private Vector3 camViewPos;
    private Quaternion camViewRot;
    private Camera playerCam;
    private CustomRigidbodyFPSController fpsController;
    //private RigidbodyFirstPersonController fpsController;

    //the camera that is used for moving and viewing
    //while operating the crane
    private Camera flyCam;

    private float startTime;
    private float journeyLength;
    private bool isCamMoving = false;
    private bool isCamRotating = false;

    //Is the claw console being used to interface with the clawrig?  
    //If so, this will take control of the button "Cancel Crane" so that 
    //the player can exit the claw system. see line 70
    private bool isActive;
    #endregion
    
    // Use this for initialization
    void Start ()
    {
        flyCam = gameObject.GetComponentInChildren<Camera>();
        camViewPos = cameraViewingPosition.position;
        camViewRot = cameraViewingPosition.rotation; 

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isActive) //if the crane is being operated
        {
            HandleCraneExitSequence();
        }   
	}

    // used to get information about our player. We get the camera and the players FPS script here.
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<CustomRigidbodyFPSController>() != null && fpsController == null)
        {
            setFPSControllerAndCamera(collision.GetComponent<CustomRigidbodyFPSController>());
        }
    }

    void HandleCraneExitSequence()
    {
        //if the player hits the Cancel Crane button 
        //and the camera is not moving or rotating
        if (Input.GetKey(KeyCode.E) 
            && (!isCamMoving || !isCamRotating))
        {
            //Turn off the claw rig
            clawRigCont.TurnRigOff(); 

            //set journeylength to be the distance the camera has to move
            journeyLength = 
                Vector3.Distance(flyCam.transform.position, 
                playerCam.transform.position);

            //set the start time
            startTime = Time.time;

            //Starts the couroutine to move and 
            //rotate the camera back to the player
            StartCoroutine(WaitForCameraToMoveAndRotate());

            //Turn off the clawConsole
            isActive = false; 

        }
    }

    IEnumerator WaitForCameraToMoveAndRotate()
    {
        //if the console is being turned on
        if (!isActive) 
        {
            //rotates the flyCam into position
            StartCoroutine(RotateCamera());
            //moves the flyCam into position
            StartCoroutine(MoveCameraToViewingPoistion());

            //this was designed to stop the code but didn't seem to work
            while (isCamMoving || isCamRotating) 
            {
                yield return null;
            }
        }
        else //if the console is being turned off
        {
            
            //moves the flyCam back to player position
            StartCoroutine(MoveCameraBackToResetPoistion());

            //switch back to player cam
            SwitchBetweenCameras();
            //give control back to the fpsController
            fpsController.enabled = true; 
        }

        yield break;
    }

    //Moves the camera up to the viewing position
    IEnumerator MoveCameraToViewingPoistion() 
    {
        isCamMoving = true; //the camera is moving

        //while the flyCam position does not equal the camViewPos position
        while (flyCam.transform.position != camViewPos) 
        {
            //Sets distcovered equal to current time 
            //minus start time times the cameraMoveSpeed
            float distCovered = (Time.time - startTime) * cameraMoveSpeed;
            //Sets the fracJourney equal to 
            //distCovered divided by the journeyLength
            float fracJourney = distCovered / journeyLength;
            //moves the flyCam according to previous calculations
            flyCam.transform.position = 
                Vector3.Lerp(flyCam.transform.position, 
                camViewPos, fracJourney);
            
            yield return null; 
        }
        
        isCamMoving = false; //the camera isn't moving

        //allows the clawRig script to start 
        //handling user input and crane movement
        clawRigCont.TurnRigOn();
        yield break;
    }

    //switches between the console camera and the player camera
    void SwitchBetweenCameras() 
    {
        flyCam.enabled = !flyCam.enabled;
        playerCam.enabled = !playerCam.enabled;
    }

    IEnumerator MoveCameraBackToResetPoistion()
    {
        isCamMoving = true; //the camera is moving

        //while the flyCam position does not equal the camViewPos position
        while (flyCam.transform.position != playerCam.transform.position)
        {
            //Sets distcovered equal to current time 
            //minus start time times the cameraMoveSpeed
            float distCovered = (Time.time - startTime) * cameraMoveSpeed;
            //Sets the fracJourney equal to distCovered divided by the journeyLength
            float fracJourney = distCovered / journeyLength;
            //moves the flyCam according to previous calculations
            flyCam.transform.position = 
                Vector3.Lerp(camViewPos, 
                playerCam.transform.position, fracJourney);

            yield return null;
        }

        isCamMoving = false; //the camera isn't moving

        yield break;
    }

    IEnumerator RotateCamera()
    {
        isCamRotating = true; //the camera is moving
        //While the flyCams rotation does not 
        //equal the cameraViewingPosition rotation

        while (flyCam.transform.rotation != camViewRot)
        {
            //Smoothly Lerps the rotation of the flyCam 
            //towards the cameraViewingPosition rotation
            flyCam.transform.rotation =
                Quaternion.Slerp(flyCam.transform.rotation,
                camViewRot, rotationSpeed * Time.deltaTime);

            yield return null;
        }

        isCamRotating = false; //the camera isn't moving

        clawRigCont.TurnRigOn();
        yield break;
    }

    public void Interact(GameObject agent)
    {
        //if the clawConsole is off
        if (!isActive) 
        {

            //move the flyCam to the player cam
            flyCam.transform.position = playerCam.transform.position;
            //Rotate the flyCam to the player cam
            flyCam.transform.rotation = playerCam.transform.rotation;

            //Switch between player cam and flycam
            SwitchBetweenCameras();

            //set the journeylength to the distance the camera has to move
            journeyLength = 
                Vector3.Distance(playerCam.transform.position, camViewPos); 

            startTime = Time.time; //set the start time

            //stops the player controller from moving
            fpsController.enabled = false;

            //start the camera movement and rotation coroutine
            StartCoroutine(WaitForCameraToMoveAndRotate()); 

            isActive = true; //Crane is in operation
        }
    }

   private void setFPSControllerAndCamera(CustomRigidbodyFPSController FPSController)
    {
        this.fpsController = FPSController;
        playerCam = fpsController.GetComponentInChildren<Camera>();

    }
    
}
