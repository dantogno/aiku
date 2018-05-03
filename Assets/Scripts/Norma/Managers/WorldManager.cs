using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {


    #region Orbits and Puzzle Dependancies

    [Header("Gameobjects that Appear/Disappear after First Scene")]

    [SerializeField]
    [Tooltip("Drag the Crowd Animation here. Enables after getting the first puzzle right.")]
    private GameObject crowd;

    [SerializeField]
    [Tooltip("Drag your Confetti Here. Enables the GameObject")]
    private GameObject confetti;

    [Header("Gameobjects that Appear/Disappear after Second Scene")]

    [SerializeField]
    [Tooltip("Drag the gameobjects that are meant to disappear from puzzle1")]
    private GameObject orbit1;

    [SerializeField]
    [Tooltip("Drag the gameobjects that are meant to disappear from puzzle1")]
    private GameObject orbit2;


	[SerializeField]
	[Tooltip("Drag the gameobjects with Norma Running Away")]
	private GameObject normaRunningAnimation;

    [SerializeField]
    [Tooltip("Drag the Gameobject with the norma crying animation")]
    private GameObject normaCrying;

	[SerializeField]
	[Tooltip("Drag the teleporting door here")]
	private GameObject teleportingDoor;


    #endregion

    [SerializeField]
    [Tooltip("Drag the lock gameobject here")]
    private GameObject Lock;

    [SerializeField]
    [Tooltip("Drag the new location of the lock here")]
    private Transform newLockLocation;

    private bool hasScenePlayed;
    private RotateWorld rotatedScript;
    private InteractCamSwitch interactedCamera;
    private GlitchyEffect incorrectInputGlitch;

    private LockInteract lockScript;
    private GameObject brackets;

    private void Awake()
    {
        hasScenePlayed = false;
        interactedCamera = Lock.GetComponent<InteractCamSwitch>();
        lockScript = Lock.GetComponent<LockInteract>();
		crowd.SetActive (false);
		normaRunningAnimation.SetActive (false);
        normaCrying.SetActive(false);
        brackets = GameObject.FindGameObjectWithTag("BracketArea");
		teleportingDoor.SetActive (false);


    }
    private void OnEnable()
    {

        RotateWorld.FirstNormaAligned += EnableFirstScene;
        RotateWorld.SecondNormaAligned += EnableSecondScene;
    }

    private void OnDisable()
    {
        RotateWorld.FirstNormaAligned -= EnableFirstScene;
        RotateWorld.SecondNormaAligned -= EnableSecondScene;

    }


    /// <summary>
    /// Upon completing the scene, animations will play and a coroutine will activate. The orbits will disable.
    /// </summary>
    private void EnableFirstScene()
    {

        if (hasScenePlayed == false)
        {
            crowd.SetActive(true);
            confetti.SetActive(true);
            crowd.GetComponent<Animator>().enabled = true;
            orbit1.SetActive(false);
            orbit2.SetActive(false);
            hasScenePlayed = true;
            StartCoroutine(FlyTheLock());

        }
    }

    /// <summary>
    /// The player is set to roam, brackets are acitve and the lock will change position from one location to the next
    /// </summary>
    private IEnumerator FlyTheLock()
    {
        lockScript.currentState = PlayerStates.Roaming;
        lockScript.LockisActive = false;
        interactedCamera.isFocused = false;
        brackets.SetActive(true);
        interactedCamera.GetOutLock();
        yield return new WaitForSeconds(.5f);
        Lock.transform.position = newLockLocation.position;
        Lock.SetActive(true);
		yield return new WaitForSeconds(3f);
		normaRunningAnimation.SetActive (true);
		yield return new WaitForSeconds(1.5f);
		normaRunningAnimation.SetActive (false);

    }
    /// <summary>
    /// This is the second scene. After Completing, a norma crying animation will play inside the house
    /// </summary>
    private void EnableSecondScene()
    {
        lockScript.currentState = PlayerStates.Roaming;
        lockScript.LockisActive = false;
        interactedCamera.isFocused = false;
        brackets.SetActive(true);
        normaCrying.SetActive(true);
		teleportingDoor.SetActive (true);

    }
		
}
