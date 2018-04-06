using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Apply this to the Pause Menu to make it a singleton.
/// Note: The Pause Menu objects needs to be a Root Game Object.
/// 
/// Warning: Do not put this on anything other than the Pause Menu.
/// If you do, there will be a fight to the death between the 
/// Pause Menu and this mysterious object that you want to make
/// into a PauseMenuSingleton for some reason.
/// </summary>
public class PauseMenuSingleton : MonoBehaviour
{
    private static PauseMenuSingleton instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
