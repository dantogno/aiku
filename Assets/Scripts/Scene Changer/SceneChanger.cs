using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script goes on an object that can be interacted with.
/// It loads or enables a new scene and disables the current one.
/// </summary>
public class SceneChanger : MonoBehaviour, IInteractable
{
    [SerializeField]
    [Tooltip("The name of the scene this object loads when Interacted with")]
    protected string sceneToLoad;

    /// <summary>
    /// When the player interacts with this object, it should Load the new scene.
    /// </summary>
    /// <param name="agentInteracting">The player interacting with the scene changing object</param>
    public virtual void Interact(GameObject agentInteracting)
    {
        SceneTransition.LoadScene(sceneToLoad);
    }
}
