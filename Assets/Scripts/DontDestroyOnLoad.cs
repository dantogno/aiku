using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is applied to any GameObject which must persist between scenes.
/// TODO: Replace this script with a more rigorous Singleton pattern.
/// </summary>

public class DontDestroyOnLoad : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
