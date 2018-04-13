using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script should go on a root game object. It turns off the gravity
/// in the world when a scene switch is initiated  and turns it back on
/// once the scene switch has finished.
/// </summary>
public class GravityManager : MonoBehaviour
{
    private Vector3 originalGravity;

    private void Awake()
    {
        // This object needs to persist through scenes.
        DontDestroyOnLoad(this);
        // Hold a reference to the original gravity.
        originalGravity = Physics.gravity;
        // Subscribe to the events where scene changes happen.
        SceneTransition.SceneChangeStarted += DisableGravity;
        SceneTransition.SceneChangeFinished += EnableGravity;
    }

    /// <summary>
    /// Turn off the gravity.
    /// </summary>
    private void DisableGravity()
    {
        originalGravity = Physics.gravity;
        Physics.gravity = Vector3.zero;
    }

    /// <summary>
    /// Turn the gravity back on.
    /// </summary>
    private void EnableGravity()
    {
        Physics.gravity = originalGravity;
    }
}
