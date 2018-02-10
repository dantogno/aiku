using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Variation on InteractableEngineScreen that exposes a couple text fields to the animator
/// </summary>
public class PowerSystemInteractableEngineScreen : InteractableEngineScreen {
    [SerializeField]
    private TextWriter usernameText, passwordText;

    public void TypeUsername()
    {
        usernameText.TypeText();
    }

    public void TypePassword()
    {
        passwordText.TypeText();
    }
}
