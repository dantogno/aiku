using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls the level's music and the distorted VO during lock interaction sequences.
/// The script is applied to a dedicated Secondary Audio Manager prefab.
/// I know it seems silly to have two audio manager scripts, but we want to keep this functionality
/// cleanly separated, to allow for ease of implementation and separation of concern.
/// </summary>

public class SecondaryAudioManager_DW : MonoBehaviour
{

    [SerializeField, Tooltip("The padlock.")]
    private LockInteract puzzleLockUpdated;

    [SerializeField, Tooltip("The first digit of this specific combination.")]
    private int militaryOfficialFirstDigit = 2, legalGuardianFirstDigit = 1, osteophosphateFirstDigit = 4;

    [SerializeField, Tooltip("The max volume level of the distorted voices.")]
    private float maxVOVolume = .5f;

    [SerializeField, Tooltip("The max volume level of the distorted voices.")]
    private float maxMusicVolume = .05f;

    [SerializeField, Tooltip("The amount of time it takes to fade audio in and out.")]
    private float fadeTime = .5f;

    #region AudioSources

    [Header("Music")]

    [SerializeField, Tooltip("The AudioSource component which plays the military official's music.")]
    private AudioSource militaryOfficialMusic;

    [SerializeField, Tooltip("The AudioSource component which plays the legal guardian's music.")]
    private AudioSource legalGuardianMusic;

    [SerializeField, Tooltip("The AudioSource component which plays Norma's bone disease music.")]
    private AudioSource osteophosphateMusic;


    [Header("Distorted VO")]

    [SerializeField, Tooltip("The AudioSource component which plays the military official's super distorted VO.")]
    private AudioSource militaryOfficialSuperDistortedVO;

    [SerializeField, Tooltip("The AudioSource component which plays the military official's slightly distorted VO.")]
    private AudioSource militaryOfficialDistortedVO;

    [SerializeField, Tooltip("The AudioSource component which plays the legal guardian's super distorted VO.")]
    private AudioSource legalGuardianSuperDistortedVO;

    [SerializeField, Tooltip("The AudioSource component which plays the legal guardian's slightly distorted VO.")]
    private AudioSource legalGuardianDistortedVO;

    [SerializeField, Tooltip("The AudioSource component which plays Norma's bone disease super distorted VO.")]
    private AudioSource osteophosphateSuperDistortedVO;

    [SerializeField, Tooltip("The AudioSource component which plays Norma's bone disease slightly distorted VO.")]
    private AudioSource osteophosphateDistortedVO;

    #endregion

    private enum GameStates { NoLocksUnlocked, FirstLockUnlocked, SecondLockUnlocked, ThirdLockUnlocked }

    private GameStates currentState = GameStates.NoLocksUnlocked;

    private bool foundFirstNumber = false;

    private void OnEnable()
    {
        puzzleLockUpdated.UsedLock += OnLockUsed;
        puzzleLockUpdated.MovedDial += CheckPadlockInput;

        WorldManager.StartedCutscene += ChangeGameState;
        EnableSecondScene.enteredSecondScene += TransitionToSecondArea;
    }
    private void OnDisable()
    {
        puzzleLockUpdated.UsedLock -= OnLockUsed;
        puzzleLockUpdated.MovedDial -= CheckPadlockInput;

        WorldManager.StartedCutscene -= ChangeGameState;
        EnableSecondScene.enteredSecondScene -= TransitionToSecondArea;
    }

    private void Update()
    { 
        ChangeCorrectNumber();
    }

    private void ChangeCorrectNumber()
    {

        if (puzzleLockUpdated.knobPlacement == 0)
        {
            militaryOfficialFirstDigit = 2;
        }

        if (puzzleLockUpdated.knobPlacement == 1)
        {
            militaryOfficialFirstDigit = 8;
        }
        if (puzzleLockUpdated.knobPlacement == 2)
        {
            legalGuardianFirstDigit = 1;
        }
        if (puzzleLockUpdated.knobPlacement == 3)
        {
            legalGuardianFirstDigit = 7;
        }

    }

    /// <summary>
    /// When the player interacts with a lock, call the appropriate method.
    /// </summary>
    private void OnLockUsed()
    {
        switch (currentState)
        {
            case GameStates.NoLocksUnlocked:
                OnMilitaryOfficialLockUsed();
                break;
            case GameStates.FirstLockUnlocked:
                OnLegalGuardianLockUsed();
                break;
            case GameStates.SecondLockUnlocked:
                OnOsteophosphateLockUsed();
                break;
        }
    }

    /// <summary>
    /// When the player lands on a number, if that number is correct, call the appropriate method.
    /// </summary>
    /// <param name="dialNumber"></param>
    private void CheckPadlockInput(int dialNumber)
    {
        switch (currentState)
        {
            case GameStates.NoLocksUnlocked:
                if (dialNumber == militaryOfficialFirstDigit)
                {
                    if (!foundFirstNumber) OnMilitaryOfficialFoundFirstNumber();
                    foundFirstNumber = true;
                }
                else
                {
                    if (foundFirstNumber)
                    {
                        OnMilitaryOfficialLostFirstNumber();

                    }
                    foundFirstNumber = false;
                }
                break;
            case GameStates.FirstLockUnlocked:
                if (dialNumber == legalGuardianFirstDigit)
                {
                    if (!foundFirstNumber) OnLegalGuardianFoundFirstNumber();
                    foundFirstNumber = true;

                }
                else
                {
                    if (foundFirstNumber)
                    {
                        OnLegalGuardianLostFirstNumber();

                    }
                    foundFirstNumber = false;
                }
                break;
            case GameStates.SecondLockUnlocked:
                if (dialNumber == osteophosphateFirstDigit)
                {
                    if (!foundFirstNumber) OnOsteophosphateFoundFirstNumber();
                    foundFirstNumber = true;
                }
                else
                {
                    if (foundFirstNumber)
                    {
                        OnOsteophosphateLostFirstNumber();
                    }
                    foundFirstNumber = false;
                }
                break;
        }
    }

    /// <summary>
    /// When a lock is unlocked, our attention shifts to the next lock, if there are any which are still locked.
    /// </summary>
    private void ChangeGameState()
    {
        switch (currentState)
        {
            case GameStates.NoLocksUnlocked:
                OnMilitaryOfficialLockUnlocked();
                currentState = GameStates.FirstLockUnlocked;
                break;
            case GameStates.FirstLockUnlocked:
                OnLegalGuardianLockUnlocked();
                currentState = GameStates.SecondLockUnlocked;
                break;
            case GameStates.SecondLockUnlocked:
                OnOsteophosphateLockUnlocked();
                currentState = GameStates.ThirdLockUnlocked;
                break;
        }
    }

    private void TransitionToSecondArea()
    {
        StartCoroutine(FadeAudio(false, militaryOfficialMusic, maxMusicVolume));

        legalGuardianMusic.Play();
        StartCoroutine(FadeAudio(true, legalGuardianMusic, maxMusicVolume));
    }

    #region Military Official

    private void OnMilitaryOfficialLockUsed()
    {
        StartCoroutine(FadeAudio(true, militaryOfficialSuperDistortedVO, maxVOVolume));
        militaryOfficialDistortedVO.volume = 0;

        militaryOfficialSuperDistortedVO.Play();
        militaryOfficialDistortedVO.Play();
    }

    private void OnMilitaryOfficialFoundFirstNumber()
    {
        StartCoroutine(FadeAudio(false, militaryOfficialSuperDistortedVO, maxVOVolume));
        StartCoroutine(FadeAudio(true, militaryOfficialDistortedVO, maxVOVolume));
    }

    private void OnMilitaryOfficialLostFirstNumber()
    {
        StartCoroutine(FadeAudio(true, militaryOfficialSuperDistortedVO, maxVOVolume));
        StartCoroutine(FadeAudio(false, militaryOfficialDistortedVO, maxVOVolume));
    }

    private void OnMilitaryOfficialLockUnlocked()
    {
        militaryOfficialSuperDistortedVO.Stop();
        militaryOfficialDistortedVO.Stop();

        militaryOfficialMusic.Play();
    }

    #endregion

    #region Legal Guardian

    private void OnLegalGuardianLockUsed()
    {
        StartCoroutine(FadeAudio(true, legalGuardianSuperDistortedVO, maxVOVolume));
        legalGuardianDistortedVO.volume = 0;

        legalGuardianSuperDistortedVO.Play();
        legalGuardianDistortedVO.Play();
    }

    private void OnLegalGuardianFoundFirstNumber()
    {
        StartCoroutine(FadeAudio(false, legalGuardianSuperDistortedVO, maxVOVolume));
        StartCoroutine(FadeAudio(true, legalGuardianDistortedVO, maxVOVolume));
    }

    private void OnLegalGuardianLostFirstNumber()
    {
        StartCoroutine(FadeAudio(true, legalGuardianSuperDistortedVO, maxVOVolume));
        StartCoroutine(FadeAudio(false, legalGuardianDistortedVO, maxVOVolume));
    }

    private void OnLegalGuardianLockUnlocked()
    {
        legalGuardianSuperDistortedVO.Stop();
        legalGuardianDistortedVO.Stop();
    }

    #endregion

    #region Osteophosphate Norma

    private void OnOsteophosphateLockUsed()
    {
        StartCoroutine(FadeAudio(true, osteophosphateSuperDistortedVO, maxVOVolume));
        osteophosphateDistortedVO.volume = 0;

        osteophosphateSuperDistortedVO.Play();
        osteophosphateDistortedVO.Play();
    }

    private void OnOsteophosphateFoundFirstNumber()
    {
        StartCoroutine(FadeAudio(false, osteophosphateSuperDistortedVO, maxVOVolume));
        StartCoroutine(FadeAudio(true, osteophosphateDistortedVO, maxVOVolume));
    }

    private void OnOsteophosphateLostFirstNumber()
    {
        StartCoroutine(FadeAudio(true, osteophosphateSuperDistortedVO, maxVOVolume));
        StartCoroutine(FadeAudio(false, osteophosphateDistortedVO, maxVOVolume));
    }

    private void OnOsteophosphateLockUnlocked()
    {
        osteophosphateSuperDistortedVO.Stop();
        osteophosphateDistortedVO.Stop();

        osteophosphateMusic.Play();
    }

    #endregion

    /// <summary>
    /// Fades audio in or out.
    /// </summary>
    private IEnumerator FadeAudio(bool fadeIn, AudioSource soundToFade, float volume)
    {
        float targetValue = fadeIn ? volume : 0,
            originalValue = fadeIn ? 0 : volume,
            elapsedTime = 0;


        while (elapsedTime < fadeTime)
        {
            soundToFade.volume = Mathf.Lerp(originalValue, targetValue, elapsedTime / fadeTime);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        soundToFade.volume = targetValue;
    }
}
