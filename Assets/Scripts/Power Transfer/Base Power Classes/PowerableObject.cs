using System;
using UnityEngine;

/// <summary>
/// A powerable object is something that the player can allocate and de-allocate power to and from.
/// Powerable objects can be powered fully on or off, or they can be in a state of partial power.
/// Most powerable objects only work if they are fully powered.
/// </summary>

public class PowerableObject : MonoBehaviour, IPowerable
{
    // These events are broadcast when the powerable object powers on or off.
    //public delegate void PowerStateChange();
    public event Action OnPoweredOn;
    public event Action OnPoweredOff;

    // Maximum amount of power a powerable object can hold. Also the amount required for the object to function as intended (e.g. for a door to open or an oven to cook).
    public int RequiredPower { get { return requiredPower; } }

    // Power currently stored in the powerable object.
    public int CurrentPower { get; protected set; }

    // This bool should return true when the current power equals the required power.
    public bool IsFullyPowered { get; protected set; }

    // If this powerable object is connected to a power switch, the number entered in this inspector field must match the number of emissive indicator lights in the power switch gameObject's children.
    [Tooltip("Amount of power required for an object to be fully powered.")]
    [SerializeField]
    protected int requiredPower = 1;

    [Tooltip("If this box is checked, the powerable object turns on when the game starts.")]
    [SerializeField]
    protected bool startsOn = true;
    
    [Tooltip("If this Inspector bool is checked, the powerable object does not turn off after the ship's generator shuts down.")]
    [SerializeField]
    protected bool retainsPowerAfterGeneratorShutdown = false;

    protected virtual void Start()
    {
        if (startsOn) PowerOn();
        else PowerOff();
    }

    /// <summary>
    /// Adds powerToAdd to CurrentPower if the result is less than RequiredPower.
    /// Powers on if the result is equal to RequiredPower.
    /// </summary>
    /// <param name="powerToAdd"></param>
    public virtual void AddPower(int powerToAdd)
    {
        if (CurrentPower + powerToAdd <= requiredPower)
            CurrentPower += powerToAdd;

        if (CurrentPower == requiredPower)
            PowerOn();
    }

    /// <summary>
    /// Subtracts powerToSubtract from CurrentPower if the result is zero or more.
    /// Powers off if the result is zero.
    /// </summary>
    /// <param name="powerToSubtract"></param>
    public virtual void SubtractPower(int powerToSubtract)
    {
        if (CurrentPower - powerToSubtract >= 0)
            CurrentPower -= powerToSubtract;

        if (CurrentPower == 0)
            PowerOff();

        // If the power level is anything less than 100%, the object is not fully powered.
        if (CurrentPower < requiredPower)
            IsFullyPowered = false;
    }

    /// <summary>
    /// Sets CurrentPower to RequiredPower.
    /// Sets IsFullyPowered to true.
    /// Invokes OnPoweredOn event.
    /// </summary>
    public virtual void PowerOn()
    {
        CurrentPower = requiredPower;
        IsFullyPowered = true;

        if (OnPoweredOn != null && IsFullyPowered)
            OnPoweredOn.Invoke();
    }

    /// <summary>
    /// Sets CurrentPower to zero.
    /// Sets IsFullyPowered to false.
    /// Invokes OnPoweredOff event.
    /// </summary>
    public virtual void PowerOff()
    {
        CurrentPower = 0;
        IsFullyPowered = false;

        if (OnPoweredOff != null && !IsFullyPowered)
            OnPoweredOff.Invoke();
    }
}
