using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawRigControlMap
{

    /// <summary>
    /// A keycode to string map created for our rig. This is to replace the old control scheme which required
    /// unity input settings to be entered. Plan on making the keys rebindable if enough time
    /// is found. 
    /// </summary>
    /// 
    public Dictionary<KeyCode, string> keysPressed { get; private set; }

    public ClawRigControlMap()
    {
        keysPressed = new Dictionary<KeyCode, string>();
        this.Initalize();
    }

    public virtual void Initalize()
    {
        keysPressed.Add(KeyCode.W, "Move Up");
        keysPressed.Add(KeyCode.S, "Move Down");
        keysPressed.Add(KeyCode.A, "Move Left");
        keysPressed.Add(KeyCode.D, "Move Right");
        keysPressed.Add(KeyCode.Mouse0, "Drop Claw");
        keysPressed.Add(KeyCode.E, "Exit Claw");
    }
}
