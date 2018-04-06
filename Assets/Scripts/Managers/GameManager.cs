using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script does two things right now: switch scenes via shortcut keys, and control the game's end state.
/// When the player has finished the levels, they should now be able to select the two crewmembers who they want to survive.
/// Apply this script to a dedicated GameObject alongside any other managers, so that it is easy to find.
/// </summary>

[RequireComponent(typeof(DontDestroyOnLoad))]
public class GameManager : MonoBehaviour
{
    // This variable tells us whether the developer shortcuts are enabled.
    private bool devToggle = false;

    private void Update()
    {
        UpdateInput();
    }

    /// <summary>
    /// Get keyboard input. Since this script is only intended to be used by us, the developers,
    /// we do not want to set up dedicated input buttons in the project's settings.
    /// </summary>
    private void UpdateInput()
    {
        // Toggle developer shortcuts.
        if (Input.GetKeyDown(KeyCode.BackQuote)) devToggle = !devToggle;

        // If developer shortcuts are toggled, switch levels by pressing the corresponding number keys.
        if (devToggle)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                SceneManager.LoadScene(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SceneManager.LoadScene(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SceneManager.LoadScene(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SceneManager.LoadScene(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SceneManager.LoadScene(4);
            }
        }
    }
}
