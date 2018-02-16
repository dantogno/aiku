using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is applied to a dedicated UI Manager GameObject.
/// Its purpose is to handle the in-game pause menu.
/// </summary>

public class PauseGame : MonoBehaviour
{
    [SerializeField] private Canvas pauseMenuCanvas;

    private void Start()
    {
        // The pause menu should be off when the game begins.
        TogglePauseMenu(false);
    }

    private void Update()
    {
        UpdatePauseInput();
    }

    private void UpdatePauseInput()
    {
        if (Input.GetButtonDown("Pause"))
            TogglePauseMenu();
    }

    #region These are publicly accessible methods for use with Button Components.

    /// <summary>
    /// Handle pause/unpause functionality.
    /// </summary>
    public void TogglePauseMenu()
    {
        bool isPaused = pauseMenuCanvas.isActiveAndEnabled;

        if (isPaused)
        {
            DeactivatePauseMenu();
            HideCursor();
            UnstopTime();
        }
        else
        {
            ActivatePauseMenu();
            ShowCursor();
            StopTime();
        }
    }

    /// <summary>
    /// Handle pause/unpause functionality.
    /// Overload method allows for more control over whether to turn the pause menu on or off.
    /// This method is accessed via the "Resume" Button Component in the pause menu's Hierarchy.
    /// </summary>
    /// <param name="turnPauseMenuOn"></param>
    public void TogglePauseMenu(bool turnPauseMenuOn)
    {
        if (turnPauseMenuOn)
        {
            ActivatePauseMenu();
            ShowCursor();
            StopTime();
        }
        else
        {
            DeactivatePauseMenu();
            HideCursor();
            UnstopTime();
        }
    }

    /// <summary>
    /// Show/hide player's "Objective Log" submenu.
    /// This method is accessed via the "Log" Button Component in the pause menu's Hierarchy.
    /// </summary>
    public void ToggleObjectiveLog()
    {
        // TODO: Add objective log functionality.
    }

    /// <summary>
    /// Exit the application.
    /// This method is accessed via the "Quit" Button Component in the pause menu's Hierarchy.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion

    private void ActivatePauseMenu()
    {
        // GameObject is activated, and Canvas Component is enabled.
        // This is not redundant, as it helps avoid "weird runtime errors." (Charles Jake Ross)
        pauseMenuCanvas.gameObject.SetActive(true);
        pauseMenuCanvas.enabled = true;
    }

    private void DeactivatePauseMenu()
    {
        // Canvas Component is disabled, and GameObject is deactivated.
        // This is not redundant, as it helps avoid "weird runtime errors." (Charles Jake Ross)
        pauseMenuCanvas.enabled = false;
        pauseMenuCanvas.gameObject.SetActive(false);
    }

    private void ShowCursor()
    {
        Cursor.visible = true;

        // Cursor can move freely around the screen.
        Cursor.lockState = CursorLockMode.None;
    }

    private void HideCursor()
    {
        Cursor.visible = false;

        // Cursor's position is locked to the middle of the screen.
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void StopTime()
    {
        Time.timeScale = 0f;
    }

    private void UnstopTime()
    {
        Time.timeScale = 1.0f;
    }
}
