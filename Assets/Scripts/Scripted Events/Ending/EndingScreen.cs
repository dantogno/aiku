using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// This class controls how the player interacts with the cryochambers after all levels have been played through.
/// The script is applied to the monitor GameObjects which are children of the level transition objects.
/// </summary>

public class EndingScreen : MonoBehaviour
{
    static int crewmembersSaved;

    // Three Actions, one for when the functionality of the scene changer should change, one for the end of the game,
    // and one for when the player transfers their power - the player's final act in the game.
    public static event Action DoneWithLevels, GameOver, TransferredHalfPowerReserve, TransferredPlayerPowerReserve;

    [SerializeField, Tooltip("Lights above cryochamber.")]
    private GameObject myLightToEnable, myLightToDisable;

    [SerializeField, Tooltip("The name of the crewmember, to appear in a prompt in the text appearing next to the monitor.")]
    private string crewmemberName;

    // Scene changer attached to this GameObject.
    private SceneChanger mySceneChanger;

    // We want to change what the text on the level transition canvasses say, so that the player knows what's up.
    private Text[] sceneChangerTexts;

    private void Awake()
    {
        InitializeReferences();
    }

    private void OnEnable()
    {
        mySceneChanger.ChoseACrewmember += OnChoseCrewmember;
        SceneChanger.SceneChangeFinished += CheckScene;
    }
    private void OnDisable()
    {
        mySceneChanger.ChoseACrewmember -= OnChoseCrewmember;
        SceneChanger.SceneChangeFinished -= CheckScene;
    }

    private void InitializeReferences()
    {
        mySceneChanger = GetComponent<SceneChanger>();
        sceneChangerTexts = GetComponentsInChildren<Text>();
    }

    /// <summary>
    /// Check how many levels the player has completed by checking how many fake switches were activated.
    /// </summary>
    private void CheckScene()
    {
        if (FakePowerSwitch.FakeSwitchesActivated == 3)
            InvokeDoneWithLevels();
    }

    /// <summary>
    /// Invokes the DoneWithLevels event and changes text next to monitor.
    /// </summary>
    private void InvokeDoneWithLevels()
    {
        if (DoneWithLevels != null) DoneWithLevels.Invoke();

        sceneChangerTexts[1].text = "TRANSFER YOUR POWER TO " + crewmemberName;
    }

    /// <summary>
    /// Fade out the text objects and disable the interactable scene changer script.
    /// If two crewmembers have been saved, roll credits!
    /// </summary>
    /// <param name="crewMember"></param>
    private void OnChoseCrewmember(string crewMember)
    {
        if (TransferredHalfPowerReserve != null) TransferredHalfPowerReserve.Invoke();

        crewmembersSaved++;

        // The screen is no longer interactable.
        GetComponent<Collider>().enabled = false;

        // Change the light, to let the player know that the cryochamber is fully powered.
        ChangeLight();

        // Turn off the text prompting player to transfer power.
        foreach (Text t in sceneChangerTexts)
        {
            StartCoroutine(FadeText(t));
        }

        // If that was the last of the player's power, die.
        if (crewmembersSaved == 2 && TransferredPlayerPowerReserve != null)
        {
            TransferredPlayerPowerReserve.Invoke();
        }
    }

    /// <summary>
    /// When a crewmember's cryochamber is fully powered, the light above their cryochamber turns blue.
    /// </summary>
    private void ChangeLight()
    {
        // Turn on blue light.
        myLightToEnable.SetActive(true);

        // Turn off magenta light.
        myLightToDisable.SetActive(false);
    }

    /// <summary>
    /// Fade out an individual text.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private IEnumerator FadeText(Text t)
    {
        Color originalColor = t.color,
            clear = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        float elapsedTime = 0, timer = 3;
        while (elapsedTime < timer)
        {
            t.color = Color.Lerp(originalColor, clear, elapsedTime / timer);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        t.color = clear;
    }
}
