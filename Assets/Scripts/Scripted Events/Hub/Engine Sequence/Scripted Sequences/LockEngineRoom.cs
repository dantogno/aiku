using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockEngineRoom : MonoBehaviour
{
    [SerializeField, Tooltip("After what event do you want the door to lock?")]
    private Task task;

    [SerializeField, Tooltip("Cargo Hold or Engine Room Door - the actual L_Door Object.")]
    private Door door1;

    [SerializeField, Tooltip("Cargo Hold or engine room door, whatever isn't above.")]
    private Door door2;

    private void OnEnable()
    {
        task.OnTaskCompleted += LockDoors;
    }
    private void OnDisable()
    {
        task.OnTaskCompleted -= LockDoors;
    }

    /// <summary>
    /// Currently, these doors will stay locked for the rest of the game.
    /// </summary>
    private void LockDoors()
    {
        door1.LockDoor();
        door2.LockDoor();
    }
}
