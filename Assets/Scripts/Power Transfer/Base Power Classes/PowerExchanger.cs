using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A power exchanger is connected to a powerable object.
/// It is the means by which the powerable object exchanges power with other powerable objects.
/// Power exchanger objects must be child objects of powerable objects.
/// Power exchangers are specialized, so this class cannot be applied directly to GameObjects. Use one of its derived classes instead.
/// </summary>

public abstract class PowerExchanger : MonoBehaviour, IInteractable
{
    // The exchanger's powerable object is accessed in order to call its publicly accessible functions.
    public PowerableObject ConnectedPowerable { get { return connectedPowerable; } }
    protected PowerableObject connectedPowerable;

    protected virtual void Awake()
    {
        // The exchanger's connected powerable must be a parent GameObject of this GameObject.
        connectedPowerable = GetComponentInParent<PowerableObject>();

        if (connectedPowerable == null)
            Debug.LogError("GameObject must be a child object of the connected powerable.");
    }

    /// <summary>
    /// Other GameObjects can interact with a power exchanger to transfer power between two powerable objects.
    /// </summary>
    /// <param name="otherObject"></param>
    public virtual void Interact(GameObject otherObject)
    {
        IPowerable otherPowerable = otherObject.GetComponentInParent<IPowerable>();

        if (otherPowerable != null) TransferPower(otherPowerable);
    }

    /// <summary>
    /// Transfer power from one powerable object to another.
    /// </summary>
    /// <param name="otherObject"></param>
    protected abstract void TransferPower(IPowerable otherObject);
}
