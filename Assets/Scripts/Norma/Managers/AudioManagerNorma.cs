using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the puzzle Audio Manager for the scene. It enables the audio when specific events were triggered
/// </summary>
public class AudioManagerNorma : MonoBehaviour {



    [SerializeField]
    [Tooltip("Drag the Audio Sources Here")]
    private AudioSource firstCutSceneAudio, secondCutSceneAudio, thirdCutSceneAudio;

    //Booleans to check if the audio has already played
    private bool hasFirstAudioPlayed = false, hasSecondAudioPlayed = false, hasThirdAudioPlayed = false;
	private bool decreaseFirstSound = false, decreaseSecondSound = false;

    private void OnEnable(){

		RotateWorld.FirstNormaAligned += EnableFirstCutscene;
		RotateWorld.SecondNormaAligned += EnableSecondCutscene;
		EnableSecondScene.enteredSecondScene += DecreaseVolumeFirstScene;
		EnableThirdScene.enteredLastScene += DecreaseVolumeSecondScene;
	}

	private void OnDisable(){
		RotateWorld.FirstNormaAligned -= EnableFirstCutscene;
		RotateWorld.SecondNormaAligned -= EnableSecondCutscene;
		EnableSecondScene.enteredSecondScene -= DecreaseVolumeFirstScene;
		EnableThirdScene.enteredLastScene -= DecreaseVolumeSecondScene;
    }
		
    void Awake () {
        firstCutSceneAudio.enabled = false;
        secondCutSceneAudio.enabled = false;
    }

    /// <summary>
    /// Enables the First CutScene Audio
    /// </summary>
	private void EnableFirstCutscene()
	{
        if (hasFirstAudioPlayed == false)
        {
            firstCutSceneAudio.enabled = true;

            if (!firstCutSceneAudio.isPlaying)

            {
                firstCutSceneAudio.Play();
            }
            
            StartCoroutine(DisableFirstSound());
        }
    }

    /// <summary>
    /// Enables the Second CutScene Audio
    /// </summary>

    private void EnableSecondCutscene()
	{
        if (hasSecondAudioPlayed == false)
        {
            secondCutSceneAudio.enabled = true;
            if (!secondCutSceneAudio.isPlaying)
            {
                secondCutSceneAudio.Play();
            }
            StartCoroutine(DisableSecondSound());
        }
    }

    /// <summary>
    /// Enables the Third CutScene Audio
    /// </summary>

    private void EnableThirdCutscene()
    {
		if (hasThirdAudioPlayed == false)
		{
			thirdCutSceneAudio.enabled = true;
			if (!thirdCutSceneAudio.isPlaying)
			{
				thirdCutSceneAudio.Play();
			}
			StartCoroutine(DisableThirdSound());
		}
    }


    /// <summary>
    /// Enumerators that take care of disabling the audio after it is finished
    /// </summary>
    private IEnumerator DisableFirstSound()
    {
        yield return new WaitForSeconds(50);
        hasFirstAudioPlayed = true;
        firstCutSceneAudio.enabled = false;
    }
    private IEnumerator DisableSecondSound()
    {
        yield return new WaitForSeconds(secondCutSceneAudio.clip.length);
        hasSecondAudioPlayed = true;
        secondCutSceneAudio.enabled = false;
    }
	private IEnumerator DisableThirdSound()
	{
		yield return new WaitForSeconds(thirdCutSceneAudio.clip.length);
		hasThirdAudioPlayed = true;
		thirdCutSceneAudio.enabled = false;
	}
		

	//Enables some booleans that triggers the decreasages of the audio
	private void DecreaseVolumeFirstScene()
	{
		decreaseFirstSound = true;
	}
	private void DecreaseVolumeSecondScene()
	{
		EnableThirdCutscene ();
		decreaseSecondSound = true;
	}
	/// <summary>		
	///Checks to see if the triggers have been entered, if so, decrease the volume of the prior cutscene
	/// </summary>
	void Update()
	{
		if (decreaseFirstSound == true) 
		
		{
			firstCutSceneAudio.volume = firstCutSceneAudio.volume -0.05f;
		}

		if (decreaseSecondSound == true) 
		{
			secondCutSceneAudio.volume = secondCutSceneAudio.volume - 0.05f;
		}

	}

}
