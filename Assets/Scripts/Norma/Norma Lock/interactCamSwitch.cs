using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this script is applied to the padlock
/// <summary>
/// This script needs to be applied to the lock. You will also need to create a seperate camera and child that under the player
/// This script is needed for the character to look at the lock object
/// </summary>

public class interactCamSwitch : MonoBehaviour,IInteractable {
	
    [SerializeField]
    [Tooltip("Drag your Character Here")]
    MonoBehaviour Player;
    Transform Target;
    float smooth = 5f;
    #region Camera declaration and initlization
    // Initialize the Main Camera in the IDE
    private Camera mainCamera;
    [SerializeField]
    [Tooltip("Place the Camera To View Lock Prefab as a child to your player and then drag it here")]
    Camera lerpingCamera;
    [Space(15)]
    [Header("Camera Bounds")]
    [SerializeField]
    bool RotationAllowed = false; //enables rotation for the level. Hub may not want rotation on
    [SerializeField]
	float MIN_X =-45f;
	[SerializeField]
	float MAX_X = 45;
	[SerializeField]
	float MIN_Y = -45;
	[SerializeField]
	float MAX_Y =45;
    #endregion
    private bool isFocused;
    private bool ifCanceled;
    float v_Axis;
    float h_Axis;
    float mouseSensitivy = 50.0f;

    void Start()
    {
        mainCamera = Camera.main;
        lerpingCamera.transform.position = mainCamera.transform.position;
        lerpingCamera.gameObject.SetActive(false);
        isFocused = false;
        gameObject.GetComponent<Lock_IInteract>().enabled = false;
    }

    public void Interact(GameObject agentInteracting)
    {
        if (isFocused == true)
        {
            //If already on focus, get out of lock      
            GetOutLock();  
        }

        else
        {
            //enables the boolean to verify the lock is on focus
            isFocused = true; 
        }
    }
    //Enables the second camera that flys towards the lock
    void _viewLock()
    {
        Player.GetComponent<Rigidbody>().isKinematic = true; //Prevents character sliding
        lerpingCamera.gameObject.SetActive(true);
        Player.enabled = false;
        gameObject.GetComponent<Lock_IInteract>().enabled = true;
        ifCanceled = false;
        CameraSwitch();
    }
    //Resets Variables and disables the second camera 
    public void GetOutLock()
    {
        Player.GetComponent<Rigidbody>().isKinematic = false;
        Player.enabled = true;
        GetComponent<Lock_IInteract>().Cancel();
        GetComponent<Lock_IInteract>().enabled = false;
        isFocused = false;
        ifCanceled = true;
		CameraSwitch();
        h_Axis = 0;
        v_Axis = 0;
    }

    void FixedUpdate()
    {
        if (isFocused)
        {
            _viewLock();
        }
    }
    //Checks to make sure what camera is currently active or not
    public void CameraSwitch()
    {
        if (ifCanceled == true)
        {
			MainCameraActive();
        }
        else
        {
			SecondCameraActive();            
        }
    }
    //Main camera gets priority, and second camera moves back towards it's original position
	private void MainCameraActive(){

        lerpingCamera.transform.position = mainCamera.transform.position;
		lerpingCamera.gameObject.SetActive(false);
	}
    //second camera gets priority, and checks whether rotation is allowed or not.
	private void SecondCameraActive(){

        Target = this.gameObject.transform.GetChild(6);
		lerpingCamera.transform.position = Vector3.Lerp(lerpingCamera.transform.position, Target.position, Time.deltaTime * smooth);
        lerpingCamera.transform.LookAt(transform.transform); //Keeps the camera in the same spot, no matter how you look at it

        if (lerpingCamera.transform.position == Target.transform.position && RotationAllowed == true) //Wait until the camera is in front of the object to allow rotation
        {
            SecondCameraInput();
        }
    }
    //If rotation is allowed, call this function
	private void SecondCameraInput()
    {
        v_Axis += Input.GetAxis("Mouse X") * mouseSensitivy * (Time.deltaTime);
        h_Axis += Input.GetAxis("Mouse Y") * mouseSensitivy * (Time.deltaTime);
        h_Axis = Mathf.Clamp(h_Axis, MIN_X, MAX_X); //Clamp added to refrain user from going too far back
        v_Axis = Mathf.Clamp(v_Axis, MIN_Y, MAX_Y);
        lerpingCamera.transform.Rotate(-h_Axis, v_Axis, 0);

    }
}
