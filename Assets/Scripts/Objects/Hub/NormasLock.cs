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

    private LockInteract padlock;

    private void Awake()
    {
        padlock = GetComponent<LockInteract>();
    }

    private void OnEnable()
    {
        padlock.Unlocked += WaitToUnlockNormasDoor;
    }
    private void OnDisable()
    {
        padlock.Unlocked -= WaitToUnlockNormasDoor;
    }
    
    private void WaitToUnlockNormasDoor()
    {
        Invoke("UnlockNormasDoor", 1);
    }

    private void UnlockNormasDoor()
    {
        normasDoor.UnlockDoor();
    }
}
