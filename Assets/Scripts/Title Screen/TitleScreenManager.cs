using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles miscellaneous title screen needs, like ensuring the lighting is correct and starting or quitting the game
/// </summary>
public class TitleScreenManager : MonoBehaviour {

    /// <summary>
    /// Do the cryochambers need to be powered off?
    /// </summary>
    private bool shouldPowerOff = true;

    /// <summary>
    /// Are we loading the opening scene?
    /// </summary>
    private bool loadingGame = false;

    [SerializeField]
    [Tooltip("The large black panel used for fading in and out of the title screen")]
    private Image loadingScreen;

    /// <summary>
    /// Loading operation for the opening scene so we can check on the progress
    /// </summary>
    private AsyncOperation loadingOperation = null;

    private void LateUpdate()
    {
        //Doing this in LateUpdate so that it happens after the power is all set up
        if (shouldPowerOff)
        {
            //Turn off the cryochambers so that they have the magenta light over them
            foreach (Cryochamber c in FindObjectsOfType<Cryochamber>())
            {
                c.PowerOff();
            }

            shouldPowerOff = false;
        }
    }

    private void Update()
    {
        //if loading the opening scene is taking a while, fade slowly so the player can tell something is going on
        if (loadingGame)
        {
            Color c = Color.clear;
            c.a = loadingOperation.progress;
        }
    }

    /// <summary>
    /// Go to the OpeningScene
    /// </summary>
    public void StartGame()
    {
        loadingGame = true;
        loadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("OpeningScene");
    }

    /// <summary>
    /// Close the application
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
