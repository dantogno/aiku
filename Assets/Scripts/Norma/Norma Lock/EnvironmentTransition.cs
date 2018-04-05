using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// EnvironmentTransition changes the Fog of the Environment
/// This requires the scene to have Fog enabled from the "Lighting" Settings with color black.
/// This also requires the lock interact, interact camera switch and rotate world scripts to work
/// </summary>


public class EnvironmentTransition : MonoBehaviour {

    #region Private booleans and floats
    private bool isSceneChange;
    //Verifies that the scene is over
    private bool finaleOnce;
    //Changes the distance of the fog based on whether the player is in the lock or not.
    private float PlayerInLock;
    private float PlayerRoaming;
    private float FogMaxLevel;
    private float intervalChange;
    private float limit;
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
	public float IntermissionTime;

	[TooltipAttribute(" Set the wait time between the decrease and increase loops")]
	[SerializeField]
	public float IntervalTime;

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
    LockInteract interact;
    InteractCamSwitch interactingwith;
    RotateWorld rotate;

    void Start()
	{
        finaleOnce = false;
        subtitleText.enabled = false;
        isSceneChange = false;
        confetti.SetActive(false);

        interactingwith = GetComponent<InteractCamSwitch>();
        rotate = GetComponent<RotateWorld>();
        interact = GetComponent<LockInteract>();

		RenderSettings.fogDensity = 50f;

        PlayerInLock = 0.001f;
		PlayerRoaming = 0f;
		FogMaxLevel = 50f;
		intervalChange = 1f;

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
        rotate = GetComponent<RotateWorld>();
        if (rotate.halfWay == true && isSceneChange == false)
        {
            //if first half of the puzzle is solved, start the fade section and first animation sequence/text.
            CheckFade();
        }

        if (rotate.finished == true && finaleOnce == false)
        {
            //if first second of the puzzle is solved, start the door opening section and end text.

            StartCoroutine("Finish"); 
        }


        if (interactingwith.allowExit == false)
        {
            if (Input.GetButtonDown("Interact"))
            {
                //Activates teh Glitch effect to show the players what they are doing is wrong. 
                StartCoroutine("Glitch");
            }
        }
            
    }
    /// <summary>
    /// Checks to see if the first half of the puzzle is solved.
    /// Activates the Fade Courotine if so
    /// </summary>

    private void CheckFade()
    {
        StartCoroutine("Fade");

    }

    /// <summary>
    ///  Fade changes the Fog level gradually
    ///  This comes after the first half of the puzzle is solved
    ///  Includes the Tracks and Animation sequence
    /// </summary>
    IEnumerator Fade()
	{
        interact.canMove = false;
        interactingwith.allowExit = false;
        Crowd.GetComponent<Animator>().enabled = true;

        isSceneChange = true;
        confetti.SetActive(true);
        subtitleText.enabled = true;
        subtitleText.text = track1;
        yield return new WaitForSecondsRealtime(WaitTime);
        subtitleText.text = track2;

        yield return new WaitForSecondsRealtime(WaitTime);
        subtitleText.text = track3;

        yield return new WaitForSecondsRealtime(WaitTime);
        subtitleText.text = track4;

        yield return new WaitForSecondsRealtime(WaitTime/2);
        subtitleText.text = "";



        if (interact.enabled == true)
        {
            limit = PlayerInLock;
        }
        else
        {
            limit = PlayerRoaming;
        }

		for(float intervalReduction = FogMaxLevel; intervalReduction > limit; intervalReduction -= intervalChange)
		{
			RenderSettings.fogEndDistance = intervalReduction;
			yield return new WaitForSecondsRealtime(IntervalTime);
		}

		// Wait the defined time, defined by the designer
		yield return new WaitForSecondsRealtime(IntermissionTime);

		// Increase the fog on a gradual interval
		for(float intervalAddition = limit; intervalAddition < FogMaxLevel; intervalAddition += intervalChange)
		{
			RenderSettings.fogEndDistance = intervalAddition;
			yield return new WaitForSecondsRealtime(IntervalTime);
            confetti.SetActive(false);
            Crowd.SetActive(false);
            orbitOne.SetActive(false);
            orbitSecond.SetActive(false);
            Puzzle1Statics.SetActive(false);
            orbitThird.SetActive(true);
            orbitFourth.SetActive(true);
            interactingwith.allowExit = true;
        }

    }

    /// <summary>
    ///  Activates the ending sequence.
    ///  This includes the end text and the door opening sequence
    /// </summary>

    IEnumerator Finish()
    {
        finaleOnce = true;
        subtitleText.text = track5;
        yield return new WaitForSecondsRealtime(WaitTime);
        subtitleText.text = track6;
        yield return new WaitForSecondsRealtime(WaitTime);
        subtitleText.text = "";
        Colliders.SetActive(true);
        DoorOpen.SetTrigger("OpenDoor");

    }

    /// <summary>
    /// Glitch effect
    /// </summary>

    IEnumerator Glitch()
    {
        rotate.secondCamera.GetComponent<GlitchyEffect>().enabled = true;
        yield return new WaitForSecondsRealtime(.3f);
        rotate.secondCamera.GetComponent<GlitchyEffect>().enabled = false;

    }


}
