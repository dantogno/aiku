﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Any object that an agent can interact with should implement this interface.
// The Player should Ray/Cone/Spherecast in order to find IInteractable objects.
// Use the "Interact" button to Interact with objects.
public interface IInteractable
{
    /// <summary>
    /// This method defines the behavior of the object when an agent interacts with it.
    /// </summary>
    void Interact(GameObject agent);
}
