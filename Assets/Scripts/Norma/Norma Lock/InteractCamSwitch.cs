using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this script is applied to the padlock
/// <summary>
/// This script needs to be applied to the lock. You will also need to create a seperate camera and child that under the player
/// This script is needed for the character to look at the lock object
/// </summary>

public class InteractCamSwitch : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Drag your Character Here")]
    private MonoBehaviour Player;

    [SerializeField]
    [Tooltip("Drag the target for the lookAt function")]
    Transform NewTarget;

    Transform Target;
    private float smooth = 5f;

    private PlayerStates currentState;
    #region Camera declaration and initlization
    // Initialize the Main Camera in the IDE
    [SerializeField]
    [Tooltip("Drag the main Camera from the current player in the scene")]
    private Camera mainCamera;

    [SerializeField]
    [Tooltip("Place the Camera To View Lock Prefab as a child to your player and then drag it here")]
    public Camera lerpingCamera;
    [Space(15)]
    [Header("Camera Bounds")]
    [SerializeField]
    [Tooltip("Allow Rotation for the camera to look around while in the lock")]
    bool RotationAllowed = false; //enables rotation for the level. Hub may not want rotation on
    [SerializeField]
    [Tooltip("How much you can view from the left")]
    float MIN_X = -45f;
    [SerializeField]
    [Tooltip("How much you can view from the right")]
    float MAX_X = 45;
    [SerializeField]
    [Tooltip("How much you can view from the down")]
    float MIN_Y = -45;
    [SerializeField]
    [Tooltip("How much you can view from the up")]
    float MAX_Y = 45;
    #endregion


    private bool ifCanceled;
    [HideInInspector]
    public bool isFocused;
    [HideInInspector]
    public bool allowExit = true;

    private float v_Axis;
    private float h_Axis;
    private float mouseSensitivy = 30.0f;

    private LockInteract LockInteractable;
    private void OnEnable()
    {
        LockInteractable.UsedLock += OnPlayerInteractedWithLock;
    }
    private void OnDisable()
    {
        LockInteractable.UsedLock -= OnPlayerInteractedWithLock;
    }

    void Awake()
    {
        lerpingCamera.transform.position = mainCamera.transform.position;
        lerpingCamera.gameObject.SetActive(false);
        isFocused = false;
        LockInteractable = GetComponent<LockInteract>();
    }

    private void OnPlayerInteractedWithLock()
    {
        if (isFocused == true && allowExit == true)
        {
            LockInteractable.currentState = PlayerStates.Roaming;
            //If already on focus, get out of lock
            StopCoroutine("SecondCameraFlyThrough");
            GetOutLock();
            isFocused = false;
        }

        else
        {
            LockInteractable.currentState = PlayerStates.UsingLock;

            //enables the boolean to verify the lock is on focus
            ViewLock();
        }
    }

    //Enables the second camera that flys towards the lock
    private void ViewLock()
    {
        Player.GetComponent<Rigidbody>().isKinematic = true; //Prevents character sliding
        lerpingCamera.gameObject.SetActive(true);
        Player.enabled = false;
        ifCanceled = false;
        CameraSwitch();
        isFocused = true;

        if (allowExit == true)
        {
            LockInteractable.currentState = PlayerStates.UsingLock;


        }
    }

    /// <summary>
    ///Resets Variables and disables the second camera 
    /// </summary>

    public void GetOutLock()
    {
        StartCoroutine(CameraFlysBack());

    }

    void FixedUpdate()
    {
        if (isFocused)
        {
            ViewLock();
        }
    }

    /// <summary>
    ///Checks to make sure what camera is currently active or not
    /// </summary>
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

    /// <summary>
    ///Main camera gets priority, and second camera moves back towards it's original position
    /// </summary>

    private void MainCameraActive()
    {
        lerpingCamera.transform.position = mainCamera.transform.position;
        lerpingCamera.gameObject.SetActive(false);
    }

    /// <summary>
    ///second camera gets priority, and checks whether rotation is allowed or not.
    /// </summary>
    private void SecondCameraActive()
    {
        StartCoroutine(SecondCameraFlyThrough());
        

        //lerpingCamera.transform.position = Vector3.Lerp(lerpingCamera.transform.position, Target.position, Time.deltaTime * smooth);
        //lerpingCamera.transform.LookAt(NewTarget); //Keeps the camera in the same spot, no matter how you look at it


    }

    /// <summary>
    /// Camera flys back to the original position
    /// After a few seconds, activates activates the original camera
    /// </summary>
    private IEnumerator SecondCameraFlyThrough()
    {
        Target = this.gameObject.transform.GetChild(5);
        Vector3 originalCamPos = lerpingCamera.transform.position;

        float elapsedTime = 0, timer = 0.2f;
        while(elapsedTime < timer)
        {
            lerpingCamera.transform.position = Vector3.Lerp(originalCamPos, Target.position, elapsedTime / timer);
            lerpingCamera.transform.LookAt(NewTarget); //Keeps the camera in the same spot, no matter how you look at it
            if (lerpingCamera.transform.position == Target.transform.position && RotationAllowed == true) //Wait until the camera is in front of the object to allow rotation
            {
                SecondCameraInput();
            }
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        lerpingCamera.transform.position = Target.position;
        lerpingCamera.transform.LookAt(NewTarget); //Keeps the camera in the same spot, no matter how you look at it
    }
    private IEnumerator CameraFlysBack()
    {
        Vector3 originalCamPos = lerpingCamera.transform.position;

        float elapsedTime = 0, timer = 0.1f;
        while (elapsedTime < timer)
        {
            lerpingCamera.transform.position = Vector3.Lerp(originalCamPos, mainCamera.transform.position, elapsedTime / timer);
            lerpingCamera.transform.LookAt(NewTarget); //Keeps the camera in the same spot, no matter how you look at it

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        lerpingCamera.transform.position = mainCamera.transform.position;
        lerpingCamera.transform.LookAt(NewTarget); //Keeps the camera in the same spot, no matter how you look at it
        Player.GetComponent<Rigidbody>().isKinematic = false;
        Player.enabled = true;
        GetComponent<LockInteract>().Cancel();
        isFocused = false;
        ifCanceled = true;
        CameraSwitch();
        h_Axis = 0;
        v_Axis = 0;
    }

    /// <summary>
    ///If rotation is allowed, call this function
    /// </summary>
    private void SecondCameraInput()
    {
        v_Axis += Input.GetAxis("Mouse X") * mouseSensitivy * (Time.deltaTime);
        h_Axis += Input.GetAxis("Mouse Y") * mouseSensitivy * (Time.deltaTime);
        h_Axis = Mathf.Clamp(h_Axis, MIN_X, MAX_X); //Clamp added to refrain user from going too far back
        v_Axis = Mathf.Clamp(v_Axis, MIN_Y, MAX_Y);
        lerpingCamera.transform.Rotate(-h_Axis, v_Axis, 0);

    }
}
