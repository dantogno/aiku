using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class unlocks Norma's stateroom when the padlock is unlocked.
/// The script is applied to the padlock on her door.
/// </summary>

public class NormasLock : MonoBehaviour
{
    [SerializeField, Tooltip("The door we want to unlock.")]
    private Door normasDoor;
    
    private void OnEnable()
    {
        // This line is commented out because the event should not be static (there are mutliple locks).
        // This script will break if we subscribe to this event now.
        //LockInteract.Unlocked += UnlockNormasDoor;
    }
    private void OnDisable()
    {
        //LockInteract.Unlocked -= UnlockNormasDoor;
    }
    
    private void UnlockNormasDoor()
    {
        normasDoor.UnlockDoor();
    }
}
