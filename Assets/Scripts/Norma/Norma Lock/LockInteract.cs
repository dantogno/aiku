using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Lock Origin Settings 
public enum LockType {NormaPuzzle, HubLock }
#endregion
/// <summary>
/// this script is applied to the padlock
/// it should be the only script that contains ANY padlock functionality at all
/// NOTE: You will also need interactCamSwitch for it to work
/// I am using Delegates that enable a function when the lock is correctly guessed
/// </summary>
public enum PlayerStates { UsingLock, Roaming }

public class LockInteract : MonoBehaviour, IInteractable
{

    #region Unity events for when the lock is completed for Norma
    public event Action UsedLock;
    public event Action<int> MovedDial;
    public event Action Unlocked;
    #endregion

    private Animator unlockAnimation;

    public PlayerStates currentState = PlayerStates.Roaming;
    private Transform currentSelection;
    private Material originalMaterial; 
    private Quaternion originalRotationValue; //remember the initial rotational value
    private int rotatedBy = 36;

    private GameObject Brackets;
	private AudioSource source;

    #region Public Variables in Inspector
    [Header("Type of Lock")]
    [SerializeField]
    [Tooltip("Select whether the lock is being used in Norma or in Hub")]
    private LockType thisLock; // change this in the editor to whichever lock this lock is (trailer, bathroom, etc.)
    [Space(10)]
    [SerializeField]
    [Tooltip("Drag the Highlighted Material here")]
    private Material highlightedMaterial; //Drag the created highlighted material here
    [SerializeField]
    [Tooltip("Select whether the lock is being used in Norma or in Hub")]
    private int ChosenPassword;
    #endregion    

    #region Hidden from Inspector but public for Norma Level Script
    [HideInInspector]
    public int[] lockNumber;
    [HideInInspector]
    public int knobPlacement;
    [HideInInspector]
    public int selectedNumber;
    [HideInInspector]
    public string passwordInput;

    public bool LockisActive = false;
    #endregion

    public void Start()
    {
        currentSelection = gameObject.transform.GetChild(0);
        originalMaterial = currentSelection.GetComponent<Renderer>().material;
        originalRotationValue = transform.GetChild(0).localRotation;
        lockNumber = new int[4] { 0, 0, 0, 0 };

        unlockAnimation = GetComponent<Animator>();
        unlockAnimation.enabled = false;

        Brackets = GameObject.FindGameObjectWithTag("BracketArea");
		source = GetComponent<AudioSource> ();
    }


    private void Update()
    {
        switch (currentState)
        {
            case PlayerStates.UsingLock:
                LockisActive = true;
                break;
            case PlayerStates.Roaming:
                LockisActive = false;
                break;
            default:
                break;
        }
        LockInteraction();
        CheckIfFinishedWithLock();
    }

    public void Interact(GameObject interactingAgent)
    {
        if (Time.timeScale == 1.0f)
        {
            if (UsedLock != null) UsedLock.Invoke();
            CheckBracketEnabled(interactingAgent);
        }

      
    }

    /// <summary>
    /// Disables the canvas renderer when interacting with the lock
    /// </summary>
    private void CheckBracketEnabled(GameObject player)
    {

        switch (currentState)
        {
            case PlayerStates.UsingLock:
                Brackets.SetActive(false);
                break;
            case PlayerStates.Roaming:
                Brackets.SetActive(true);
                break;
            default:
                break;
        }



    }

    /// <summary>
    ///Resets the placement of the lock knob to the first knob. Resets the original material, and disables this script. This is called by the interactCamSwitch.
    /// </summary>
    public void Cancel()
    {
        knobPlacement = 0;
        currentSelection.gameObject.GetComponent<Renderer>().material = originalMaterial;
    }
    /// <summary>
    ///The rotational input of each knob, as well which knob is interacted with and updating the passwordinput as we go
    /// </summary>
    private void LockInteraction()
    {
        if (LockisActive == true)
        {
            //Combine the arrays into one string to verify input at the end
            passwordInput = "" + lockNumber[0] + lockNumber[1] + lockNumber[2] + lockNumber[3];
            currentSelection = gameObject.transform.GetChild(knobPlacement);
            currentSelection.gameObject.GetComponent<Renderer>().material = highlightedMaterial;

            //Button Mapping and the math needed to know what number is currently being selected
            if (Input.GetButtonDown("Horizontal") && Input.GetAxis("Horizontal") < 0)
            {
				
				source.Play ();
                currentSelection.transform.Rotate(0, -rotatedBy, 0);
                if (selectedNumber < 10)
                {
                    selectedNumber--;
                }

                if (selectedNumber < 0)
                {
                    selectedNumber = 9;
                }

                lockNumber[knobPlacement] = selectedNumber;

                if (MovedDial != null) MovedDial.Invoke(lockNumber[0]);
            }
            if (Input.GetButtonDown("Horizontal") && Input.GetAxis("Horizontal") > 0)
            {
				
				source.Play ();

                currentSelection.transform.Rotate(0, rotatedBy, 0);
                if (selectedNumber < 10)
                {
                    selectedNumber++;
                }
                if (selectedNumber == 10)
                {
                    selectedNumber = 0;
                }
                lockNumber[knobPlacement] = selectedNumber;

                if (MovedDial != null) MovedDial.Invoke(lockNumber[0]);
            }

            //Going up and down the knobs of the lock 
            if (Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") != 0)
            {
				source.Play ();


                if (Input.GetAxis("Vertical") < 0)
                {

                    if (knobPlacement < 3)
                    {
                        knobPlacement++;
                    }

                    else
                    {
                        knobPlacement = 0; //resets to the first lock knob
                        currentSelection.gameObject.GetComponent<Renderer>().material = originalMaterial;
                    }

                    selectedNumber = lockNumber[knobPlacement]; //Calls back the value registered to that knob
                }
                else if (Input.GetAxis("Vertical") > 0)
                {
                    if (knobPlacement < 4 && knobPlacement > 0)
                    {
                        knobPlacement--;
                        selectedNumber = lockNumber[knobPlacement];
                    }

                    else
                    {
                        currentSelection.gameObject.GetComponent<Renderer>().material = originalMaterial;
                    }
                }
                currentSelection.gameObject.GetComponent<Renderer>().material = originalMaterial;
            }
        }
    }

    /// <summary>
    ///Calls for the LockType Enums.  
    /// </summary>
    private void CheckIfFinishedWithLock()
    {
        switch (thisLock)
        {


            case LockType.HubLock:

                if (passwordInput == "2817" && Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") < 0 && knobPlacement == 0) // After it resets, call it a win

                    FinishWithLock();
                break;
            case LockType.NormaPuzzle:
                if (passwordInput == "2817" && LockisActive == true) // After it resets, call it a win

                    FinishWithLock();
                break;


        }

    }


    /// <summary>
    /// called when the player gets the number right. Subscribe to the event on your seperate script (for hub) 
    /// </summary>
    private void FinishWithLock()
    {
        /// <summary>
        /// do something special, depending on which lock it is
        /// calls player state if all the numbers are inputed. this disables the script so  users are not able to move the lock after completing all the numbers
        /// 
        /// </summary>
        switch (thisLock)
        {
            case LockType.NormaPuzzle:
                currentState = PlayerStates.Roaming;
                if (Unlocked != null) Unlocked.Invoke();
                
                break;
            case LockType.HubLock:
                currentState = PlayerStates.Roaming;
                if (Unlocked != null) Unlocked.Invoke();
                break;
            default:
                break;        
        }
        LockisActive = false;
        StartCoroutine(UnlockedCoroutine());
        //Gets out of the lock sequence

    }

    /// <summary>
    /// After completing the lock, the animation sequence is enabled and plays.
    /// This also activates the smooth fly back from the interact camera switch
    /// </summary>
    private IEnumerator UnlockedCoroutine()
    {
        unlockAnimation.enabled = true;
        yield return new WaitForSecondsRealtime(1f);

        GetComponent<InteractCamSwitch>().GetOutLock();
        //Disables the script to go back to the lock
        GetComponent<InteractCamSwitch>().enabled = false;
        yield return new WaitForSecondsRealtime(1f);

    }
}
