using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for interactable valves, for interacting with them.
/// The script is applied to interactable valves. Summaries for physical object classes are so easy!
/// </summary>

public class EngineRoomValve : MonoBehaviour, IInteractable
{
    // The different things the player can do with valves.
    public event Action ClosedValve, OpenedValve, ClosedButterflyValve;

    public bool Closed { get { return closed; } }

    private enum ValveType { Twistable, Butterfly }

    [SerializeField, Tooltip("What kind of valve is it?")]
    private ValveType valveType = ValveType.Twistable;

    // Is the valve closed or open?
    private bool closed = false;

    public void Interact(GameObject interactingAgent)
    {
        TwistValve();
    }

    /// <summary>
    /// Play animation, check the valve type and react accordingly, disable interaction, and play a sound.
    /// </summary>
    private void TwistValve()
    {
        closed = true;

        // Play twist animation.
        Animator myAnimator = GetComponent<Animator>();
        myAnimator.SetTrigger("Twist");

        #region Check which kind of valve it is, and invoke the appropriate event.

        if (ClosedValve != null)
        {
            ClosedValve.Invoke();

            if (valveType == ValveType.Butterfly && ClosedButterflyValve != null)
            {
                ClosedButterflyValve.Invoke();
            }
        }

        #endregion

        // Disable collision, and therefore interaction.
        Collider myCollider = GetComponent<Collider>();
        myCollider.enabled = false;

        #region Play sound.

        if (GetComponent<AudioSource>() != null)
        {
            if (valveType == ValveType.Twistable) GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(.5f, 1.5f);
            else GetComponent<AudioSource>().pitch = .25f;
            GetComponent<AudioSource>().Play();
        }

        #endregion
    }
}
