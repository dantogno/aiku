using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is used to deactivate the cone lighting in the scene once the player walks close enough to the lock
/// Attach this script to a gameobject set to Trigger
/// </summary>
public class TriggerLights : MonoBehaviour {

    //Cone light that is set to deactivate after triggerenter
    [SerializeField]
    [Tooltip("Drag the Fake Volumetric Light in this field. Disables the light once the player collides w ith Trigger")]
    private GameObject lightCone;

    private void OnTriggerEnter(Collider other)
    {
        lightCone.SetActive(false);

    }
}
