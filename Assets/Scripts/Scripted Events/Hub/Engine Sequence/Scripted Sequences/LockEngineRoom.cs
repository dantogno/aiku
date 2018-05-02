using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class exists to lock the player in the engine room during the generator-checking sequence,
/// to prevent them from getting distracted by the opening door when they come near.
/// The script is applied to a dedicated GameObject or the task on which it depends.
/// </summary>

public class LockEngineRoom : MonoBehaviour
{
    [SerializeField, Tooltip("After what event do you want the door to lock?")]
    private Task task, sosTask;

    [SerializeField, Tooltip("Cargo Hold or Engine Room Door - the actual L_Door Object.")]
    private Door door1;

    [SerializeField, Tooltip("Cargo Hold or engine room door, whatever isn't above.")]
    private Door door2;

    private void OnEnable()
    {
        task.OnTaskCompleted += LockDoors;
        sosTask.OnTaskCompleted += UnlockDoors;
    }
    private void OnDisable()
    {
        task.OnTaskCompleted -= LockDoors;
        sosTask.OnTaskCompleted -= UnlockDoors;
    }

    /// <summary>
    /// Lock the doors when the player enters the engine room for the generator sequence.
    /// </summary>
    private void LockDoors()
    {
        door1.LockDoor();
        door2.LockDoor();
    }

    /// <summary>
    /// Unlock the doors when the player gets out of the elevator.
    /// </summary>
    private void UnlockDoors()
    {
        door1.UnlockDoor();
        door2.UnlockDoor();
    }
}
