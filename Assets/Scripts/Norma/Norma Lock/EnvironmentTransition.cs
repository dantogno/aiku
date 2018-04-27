using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// EnvironmentTransition changes the Fog of the Environment and triggers Animation Sequences after successfully solving the puzzles
/// This requires the scene to have Fog enabled from the "Lighting" Settings with color black.
/// This also requires the lock interact, interact camera switch and rotate world scripts to work
/// </summary>


public class EnvironmentTransition : MonoBehaviour {

    #region Private booleans and floats

    //Checks to see if the transitions are active or not. Prevents the courotine from playing more than once. 
    private bool isFadeTransitionPlaying = false;
    //This is the final transition scene. Checks whether the scene is done.
    private bool isFinalTransitionPlaying = false;
    //Changes the distance of the fog based on whether the player is in the lock or not.
    private float PlayerInLock = 0.001f;
    private float PlayerRoaming = 0f;
    //Sets the max value for Fogs
    private float FogMaxLevel = 50f;
    //The total time elapsed the Fog Transition should take
    private float totalTimeFogTransition = 1f;
    //Distance between the player and the fog. Visibility altered whether player is in lock or exploring while fog is in effect
    private float distanceBeforeFogStop;
    #endregion

    #region Orbits

    //These are the orbits that are enabled/disabled during transition
    private GameObject orbitOne;
    private GameObject orbitSecond;
    private GameObject orbitThird;
    private GameObject orbitFourth;

    #endregion

    #region Orbits and Puzzle Dependancies

    [Header("Gameobjects that Appear/Disappear after First Scene")]

    [SerializeField]
    [Tooltip("Drag the gameobjects that are meant to disappear from puzzle1")]
    private GameObject Puzzle1Statics;

    [SerializeField]
    [Tooltip("Drag the Crowd Animation here. Enables after getting the first puzzle right.")]
    private GameObject Crowd;

    [SerializeField]
    [Tooltip("Drag your Confetti Here. Enables the GameObject")]
    private GameObject confetti;

    [Header("Gameobjects that Appear/Disappear after Second Scene")]

    [SerializeField]
    [Tooltip("Drag the Door Animation here")]
    private Animator DoorOpen;

    [SerializeField]
    [Tooltip("Drag your Colliders Here. This enables the ramp for the final scene to exit the Norma Level")]
    private GameObject Colliders;


    #endregion
    [Space(10)]

    #region Scene Switch Dependancies
    [Header("Settings for the Scene Switch")]

    [TooltipAttribute(" Set the wait time between decrease and increase of fog value")]
	[SerializeField]
	private float IntermissionTime;

	[TooltipAttribute(" Set the wait time between the decrease and increase loops")]
	[SerializeField]
	private float IntervalTime;

    #endregion
    [Space(10)]

    #region Subtitle Track
    [Header("Subtitles Track")]

    [SerializeField]
    [Tooltip("Time between track 1 and track 2")]
    private int WaitTime;

    [SerializeField]
    [Tooltip("Drag the text field from the canvas here")]
    private Text subtitleText;
    [SerializeField]
    [Tooltip("First Track to Play goes here")]
    private string track1;
    [SerializeField]
    [Tooltip("Second Track to Play goes here")]
    private string track2;
    [SerializeField]
    [Tooltip("Third Track to Play goes here")]
    private string track3;
    [SerializeField]
    [Tooltip("Fourth Track to Play goes here")]
    private string track4;
    [SerializeField]
    [Tooltip("Fifth Track to Play goes here")]
    private string track5;
    [SerializeField]
    [Tooltip("Sixth Track to Play goes here")]
    private string track6;

    #endregion


    // Calls the other scripts to check if all booleans are in check and to hide/enable gameobjects
    private LockInteract interact;
    private InteractCamSwitch interactingwith;
    private RotateWorld rotate;
    private GlitchyEffect incorrectInputGlitch;

    void Start()
	{
        InitilializeReferences();
        AssignVariables();
        SetFogDesity();
        DisableConfetti();
        DisableSubtitles();        
	}

    /// <summary>
    /// Initializes each variable that depend on the lockInteraction, RotateWorld, Interact and Glitch Effect
    /// </summary>
    private void InitilializeReferences()
    {
        interactingwith = GetComponent<InteractCamSwitch>();
        rotate = GetComponent<RotateWorld>();
        interact = GetComponent<LockInteract>();

    }

    private void AssignVariables()
    {
        incorrectInputGlitch = rotate.GetComponent<GlitchyEffect>();
        //Assign the orbits from the once used in the RotateWorld Script
        orbitOne = rotate.RotatedFirst;
        orbitSecond = rotate.RotatedSecond;
        orbitThird = rotate.RotatedThird;
        orbitFourth = rotate.RotatedFourth;

    }

    /// <summary>
    /// Checks to see whther the puzzle is finished or not.
    /// This also prevents the player from leaving the lock mid-puzzle
    /// </summary>
    void FixedUpdate()
	{
        if (rotate.Halfway == true && isFadeTransitionPlaying == false)
        {
            //if first half of the puzzle is solved, start the fade section and first animation sequence/text.
            StartFade();
        }

        if (rotate.Finished == true && isFinalTransitionPlaying == false)
        {
            //if first second of the puzzle is solved, start the door opening section and end text.
			interact.LockisActive = false;
			interact.currentState = PlayerStates.Roaming;

            StartCoroutine(Finish()); 
        }

        if (interactingwith.allowExit == false)
        {
            if (Input.GetButtonDown("Interact"))
            {
                //Activates the Glitch effect to show the players what they are doing is wrong. 
                StartCoroutine(Glitch());
            }
        }          
    }
    /// <summary>
    /// Calls the Courotine
    /// </summary>

    private void StartFade()
    {
        StartCoroutine(Fade());
    }

    /// <summary>
    ///  Fade changes the Fog level gradually
    ///  This comes after the first half of the puzzle is solved
    /// </summary>
    private IEnumerator Fade()
	{
        isFadeTransitionPlaying = true;
        PauseLockInteraction();
        EnableCrowds();
        EnableConfetti();

        if (interact.enabled == true)
        {
            distanceBeforeFogStop = PlayerInLock;
        }
        else
        {
            distanceBeforeFogStop = PlayerRoaming;
        }

        for (float FogTransitionReduction = FogMaxLevel; FogTransitionReduction > distanceBeforeFogStop; FogTransitionReduction -= totalTimeFogTransition)
        {
            RenderSettings.fogEndDistance = FogTransitionReduction;
            yield return new WaitForSecondsRealtime(IntervalTime);
        }

        // Wait the defined time, defined by the designer
        yield return new WaitForSecondsRealtime(IntermissionTime);
  
        // Increase the fog on a gradual interval
        for (float FogTransitionAddition = distanceBeforeFogStop; FogTransitionAddition < FogMaxLevel; FogTransitionAddition += totalTimeFogTransition)
        {
            RenderSettings.fogEndDistance = FogTransitionAddition;
            yield return new WaitForSecondsRealtime(IntervalTime);

            DisableConfetti();
            DisableCrowds();
            interactingwith.allowExit = true;
        }
        EnableSecondEnvironment();
    }


    /// <summary>
    /// The region below includes all the Methods that are seperated to enable or disable gameobjects/animations
    /// </summary>
    #region Methods that enable and disable gameObjects
    private void PauseLockInteraction()
    {
		//During cutsscene, the player cannot interact with lock. Nor can they exit
		interact.currentState = PlayerStates.Roaming;
        interactingwith.allowExit = false;
    }

    private void EnableCrowds()
    {
        Crowd.GetComponent<Animator>().enabled = true;
    }

    private void DisableCrowds()
    {
        Crowd.SetActive(false);
    }
    private void DisableSubtitles()
    {
        subtitleText.enabled = false;
    }
    private void DisableConfetti()
    {
        confetti.SetActive(false);
    }
    private void EnableConfetti()
    {
        confetti.SetActive(true);
    }
    private void SetFogDesity()
    {
        RenderSettings.fogDensity = 50f;
    }
    private void BackToTheHub()
    {
        DoorOpen.SetTrigger("OpenDoor");
        Colliders.SetActive(true);
    }
    private void EnableSecondEnvironment()
    {
        orbitOne.SetActive(false);
        orbitSecond.SetActive(false);
        Puzzle1Statics.SetActive(false);
        orbitThird.SetActive(true);
        orbitFourth.SetActive(true);
    }

    #endregion



    /// <summary>
    ///  Activates the ending sequence.
    ///  This includes the subtitles
    /// </summary>
    private IEnumerator Finish()
    {
        yield return new WaitForSecondsRealtime(3f);
        //Goes back to the hub. This enables the door animation and the ramp towards the computer
        BackToTheHub();
    }

    /// <summary>
    /// Glitch effect that denots whether the input the user made is incorrect or not 
    /// </summary>

    private IEnumerator Glitch()
    {
        incorrectInputGlitch.enabled = true;
        yield return new WaitForSecondsRealtime(.3f);
        incorrectInputGlitch.enabled = false;

    }
}
