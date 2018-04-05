using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWorld : MonoBehaviour
{
    /// <summary>
    /// This script rotates the scene. The environment transition, lock interact and interact cam switch
    /// Set the gameobjects for each orbit in the inspector
    /// </summary>


    #region Orbits for the Puzzle Sequence
    [Header("Orbits for Puzzle")]

    [SerializeField]
    [Tooltip("Drag the first Orbit here")]
    public GameObject RotatedFirst;

    [SerializeField]
    [Tooltip("Drag the second Orbit here")]
    public GameObject RotatedSecond;

    [SerializeField]
    [Tooltip("Drag the third Orbit here")]
    public GameObject RotatedThird;

    [SerializeField]
    [Tooltip("Drag the Fourth Orbit here")]
    public GameObject RotatedFourth;
    #endregion


    //Set as public so that EnvironmentTransition can access these variables
    #region Hidden Public Variables for the EnvironmentTransition script
    [HideInInspector]
    public int knob;
    [HideInInspector]
    public bool halfWay;
    [HideInInspector]
    public bool finished;
    [HideInInspector]
    public Camera secondCamera;
    #endregion

    [Space(10)]

    //Materials that change based on the status of the lock
    #region Material Region
    [Header("Materials")]

    [SerializeField]
    [Tooltip("This is the restricted material. darkens the knobs that are not allowed to be moved")]
    private Material RestrictedMaterial;

    [SerializeField]
    [Tooltip("This is the original materials fo the knobs")]
    private Material originalMaterial;
    #endregion

    //Private integers for the degrees for rotation and the password
    #region Ints including the rotation angle and the password

    private int rotatedAngle = 36;
    private int firstPass = 2;
    private int secondPass = 8;
    private int thirdPass = 1;
    private int fourthPass = 7;
    private int currentNumber;

    #endregion

    //Calls for the lock scripts 
    LockInteract lockscript;
    InteractCamSwitch interact;


    void Start()
    {
        halfWay = false;
        finished = false;
        lockscript = GetComponent<LockInteract>();
        interact = GetComponent<InteractCamSwitch>();
        secondCamera = interact.lerpingCamera;
        secondCamera.GetComponent<GlitchyEffect>().enabled = false;
        secondCamera = interact.lerpingCamera;

    }

    private void FixedUpdate()
    {

        currentNumber = lockscript.selectedNumber;
        knob = lockscript.knobPlacement;
        
        CheckStatus();
        RestrictLock();
        RotateObject();
    }

    /// <summary>
    /// This sections overrides the material and the rotation of the scene depending on the status
    /// </summary>
    private void CheckStatus()
    {
        //If the lock is finished, default the second half of the orbits to the correct position 
        if (finished == true)
        {
            if (RotatedThird != null && RotatedFourth != null)
            {
                RotatedThird.transform.localRotation = Quaternion.Euler(0, 0, 0);
                RotatedFourth.transform.localRotation = Quaternion.Euler(0, 0, 0);

            }
        }
        //If the first two numbers are correct, then set the first two orbits to the correct position
        if (halfWay == true)
        {
            RotatedFirst.transform.localRotation = Quaternion.Euler(0, 0, 0);
            RotatedSecond.transform.localRotation = Quaternion.Euler(0, 0, 0);

            //Clamp what locks you can move. Only 2 and three once halfway boolean is true
            lockscript.knobPlacement = Mathf.Clamp(lockscript.knobPlacement, 2, 3);

            //Swap the materials out and override the rotation for the first two knobs 
            if (lockscript.enabled == true)
            {
                LockInteract.UsingPuzzle += UsingPuzzle;
                lockscript.lockNumber[0] = firstPass;
                lockscript.lockNumber[1] = secondPass;
                gameObject.transform.GetChild(0).GetComponent<Renderer>().material = RestrictedMaterial;
                gameObject.transform.GetChild(1).GetComponent<Renderer>().material = RestrictedMaterial;
                gameObject.transform.GetChild(2).GetComponent<Renderer>().material = originalMaterial;
                gameObject.transform.GetChild(3).GetComponent<Renderer>().material = originalMaterial;

            }
        }

        //By default, the last two knobs should be restricted
        if (halfWay == false && lockscript.enabled == true)
        {
            gameObject.transform.GetChild(2).GetComponent<Renderer>().material = RestrictedMaterial;
            gameObject.transform.GetChild(3).GetComponent<Renderer>().material = RestrictedMaterial;

        }
    }

    /// <summary>
    /// The knobs rotates the world by 36 degrees. There is also a snapping feature in case the player decides to interrup the lerp.
    /// </summary>
    private void RotateObject()
    {

            if (knob == 0)
            {
                if (RotatedFirst != null)
                {
                    RotatedFirst.transform.localRotation = Quaternion.Lerp(RotatedFirst.transform.localRotation, Quaternion.Euler(0, 36 * (currentNumber - firstPass), 0), Time.deltaTime * 6f);
                    RotatedSecond.transform.localEulerAngles = new Vector3(0, 0 + rotatedAngle * (lockscript.lockNumber[1] - secondPass), 0); //In case the player doesnt wait for the lerp to finish
                }
            }
            if (knob == 1)
            {
                if (RotatedSecond != null)
                {

                    RotatedFirst.transform.localEulerAngles = new Vector3(0, 0 + rotatedAngle * (lockscript.lockNumber[0] - firstPass), 0);  //In case the player doesnt wait for the lerp to finish

                    RotatedSecond.transform.localRotation = Quaternion.Lerp(RotatedSecond.transform.localRotation, Quaternion.Euler(0, rotatedAngle * (currentNumber - secondPass), 0), Time.deltaTime * 6f);

                }

            }
            if (knob == 2)
            {
                if (RotatedThird != null)
                {
                    RotatedThird.transform.localRotation = Quaternion.Lerp(RotatedThird.transform.localRotation, Quaternion.Euler(0, 36 * (currentNumber - thirdPass), 0), Time.deltaTime * 6f); 
                    RotatedFourth.transform.localEulerAngles = new Vector3(0, 0 + (rotatedAngle) * (lockscript.lockNumber[3] - fourthPass), 0);//In case the player doesnt wait for the lerp to finish

            }
            }
            if (knob == 3)
            {
                if (RotatedFourth != null)
                {
                    RotatedThird.transform.localEulerAngles = new Vector3(0, 0 + rotatedAngle * (lockscript.lockNumber[2] - thirdPass), 0); //In case the player doesnt wait for the lerp to finish
                    RotatedFourth.transform.localRotation = Quaternion.Lerp(RotatedFourth.transform.localRotation, Quaternion.Euler(0, (rotatedAngle) * (currentNumber - fourthPass), 0), Time.deltaTime * 6f);
                }
            }
        }

    /// <summary>
    /// This sections restricts the lock from going up to the first two knobs if the first half is already correct
    /// In also calls to the courotine of the glitch animation in case to let the player know that they cannot access those knobs
    /// </summary>

    private void RestrictLock()
    {
        if (lockscript.knobPlacement == 2)
        {
            if (lockscript.passwordInput == "2800")
            {
                halfWay = true;

            }
            else
            {
                if (halfWay == false)
                {
                    StartCoroutine("Glitch");
                    lockscript.knobPlacement = 0;
                }
                else
                {
                    if (lockscript.knobPlacement == 2 && Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") > 0)
                    {
                        lockscript.knobPlacement = 0;
                    }
                }
            }
        }

            lockscript.selectedNumber = lockscript.lockNumber[lockscript.knobPlacement];
        //When they finish, they can press the interact button or go down and it will activate teh final sequence

        if (lockscript.knobPlacement == 3 && Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") < 0 && lockscript.passwordInput != "2817")
        {
            StartCoroutine("Glitch");
            lockscript.knobPlacement = 0;
        }

        if (lockscript.passwordInput == "2817")
        {
            if (Input.GetButtonDown("Interact"))
            {
                UsingPuzzle();
            }
        }
    }
    /// <summary>
    /// Disables the script to access the interaction to the lock after completions
    /// </summary>
    private void UsingPuzzle()
    {
        finished = true;
        RotatedFourth.transform.localEulerAngles = new Vector3(0, 0 + rotatedAngle * (lockscript.lockNumber[3] - thirdPass), 0);
        interact.enabled = false;
    }

    /// <summary>
    /// Glitch animation sequence to express the notion that their action is wrong
    /// </summary>
    IEnumerator Glitch()
    {
        secondCamera.GetComponent<GlitchyEffect>().enabled = true;
        yield return new WaitForSecondsRealtime(.3f);
        secondCamera.GetComponent<GlitchyEffect>().enabled = false;
    }

}
