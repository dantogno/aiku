using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class activates the level transition screens.
/// The script is applied to the parent object of the level transition monitors, or to any dedicated GameObject.
/// </summary>

public class LevelTransitionParentObject : MonoBehaviour
{
    [SerializeField, Tooltip("The monitors which activate when the player can enter levels.")]
    private GameObject[] levelTransitionMonitors;

    [SerializeField, Tooltip("The monitors which deactivate when the player can enter levels.")]
    private GameObject[] ordinaryMonitors;

    private void OnEnable()
    {
        BottomStairsPowerSwitch.ExchangedPower += ActivateLevelTransitionMonitors;
    }
    private void OnDisable()
    {
        BottomStairsPowerSwitch.ExchangedPower -= ActivateLevelTransitionMonitors;
    }

    /// <summary>
    /// Disable the ordinary monitors and enable the level transition monitors.
    /// </summary>
    private void ActivateLevelTransitionMonitors()
    {
        foreach (GameObject g in levelTransitionMonitors) g.SetActive(true);
        foreach (GameObject g in ordinaryMonitors) g.SetActive(false);
    }
}
