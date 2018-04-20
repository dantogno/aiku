using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script goes on any object that can be interacted with.
/// It disables any number of gameobjects at the start, and
/// enables them again when t is interacted with.
/// </summary>
public class ActivateObjects : MonoBehaviour, IInteractable
{
    [SerializeField]
    [Tooltip("The GameObjects that should be enabled when the player interacts with this object.")]
    private GameObject[] objectsToActivate;


    /// <summary>
    /// Disables all GameObjects as specified in the editor.
    /// </summary>
    protected virtual void Start()
    {
        // Start by disabling all of the objects.
        foreach (GameObject gameObject in objectsToActivate)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Enables all GameObjects as specified in the editor.
    /// </summary>
    /// <param name="agentInteracting"></param>
    public virtual void Interact(GameObject agentInteracting)
    {
        // Enable all of the objects
        foreach(GameObject gameObject in objectsToActivate)
        {
            gameObject.SetActive(true);
        }
    }
}
