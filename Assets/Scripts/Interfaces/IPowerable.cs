using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Powerable objects should implement this interface.
/// This defines the properties and functionality of a powerable object.
/// Powerable objects are interacted with via power exchangers, which in turn implement IInteractable.
/// </summary>

public interface IPowerable
{
    /// <summary>
    /// Amount of power necessary for object to be fully powered.
    /// </summary>
    int RequiredPower { get; }
    
    int CurrentPower { get; }

    bool IsFullyPowered { get; }

    /// <summary>
    /// Sets CurrentPower to RequiredPower.
    /// </summary>
    void PowerOn();

    /// <summary>
    /// Fully depletes CurrentPower.
    /// </summary>
    void PowerOff();
    
    /// <summary>
    /// Adds power to CurrentPower.
    /// </summary>
    /// <param name="power"></param>
    void AddPower(int power);

    /// <summary>
    /// Subtracts power from CurrentPower.
    /// </summary>
    /// <param name="power"></param>
    void SubtractPower(int power);
}
