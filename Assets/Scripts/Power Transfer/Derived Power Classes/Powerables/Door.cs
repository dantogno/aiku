using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Doors in this game are automatic. When powered, the door will activate via player proximity.
/// The logic and data within this class are considered to be mostly self-explanatory, hence the lack of documentation.
/// </summary>

[RequireComponent(typeof(Animator))]

public class Door : PowerableObject
{
    [Tooltip("If this box is checked, the door will not open.")]
    [SerializeField]
    private bool locked = false;

    private Animator myAnimator;

    private bool open = false;

    protected override void Start()
    {
        base.Start();

        myAnimator = GetComponent<Animator>();
    }

    /// <summary>
    /// Sets CurrentPower to zero.
    /// Sets IsFullyPowered to false.
    /// Invokes OnPoweredOff event.
    /// Closes door.
    /// </summary>
    public override void PowerOff()
    {
        base.PowerOff();

        if (!IsFullyPowered && open) CloseDoor();
    }

    private void OnTriggerStay(Collider other)
    {
        bool canOpen = IsFullyPowered && !open && !locked;

        if (canOpen) OpenDoor();
    }

    private void OnTriggerExit(Collider other)
    {
        bool canClose = open && !locked;

        if (canClose) CloseDoor();
    }

    private void OpenDoor()
    {
        myAnimator.SetTrigger("Open");
        open = true;
    }

    private void CloseDoor()
    {
        myAnimator.SetTrigger("Close");
        open = false;
    }

    public void UnlockDoor()
    {
        locked = false;
    }
}
