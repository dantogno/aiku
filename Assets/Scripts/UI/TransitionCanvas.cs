using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is a singleton. It goes on the canvas that is enabled
/// when the player enters a level from the Hub.
/// </summary>
public class TransitionCanvas : MonoBehaviour
{
    private TransitionCanvas instance;

    private void Awake()
    {
        // This starts inactive.
        this.gameObject.SetActive(false);
        // Make sure this object is a singleton
        bool exists = CheckForSingleton();
        if(exists)
        {
            // If it is a singleton, we don't want to subscribe.
            return;
        }
        // Subscribe to scene transitions.
        SceneTransition.SceneChangeStarted += EnableThis;
        SceneTransition.SceneChangeFinished += DisableThis;
    }

    /// <summary>
    /// Ensures that this object is a singleton.
    /// </summary>
    /// <returns></returns>
    private bool CheckForSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            return false;
        }
        else
        {
            Destroy(this.gameObject);
            return true;
        }
    }

    /// <summary>
    /// Enables the canvas if the player is loading a level from the Hub.
    /// </summary>
    private void EnableThis()
    {
        if(SceneTransition.CurrentScene == "Hub")
        {
            this.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Disables the canvas.
    /// </summary>
    private void DisableThis()
    {
        this.gameObject.SetActive(false);
    }
}
