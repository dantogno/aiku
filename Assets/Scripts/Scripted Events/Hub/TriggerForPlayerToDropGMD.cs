using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class forces the player to drop the GMD when they enter the lab.
/// The script is applied to the trigger justb inside the lab.
/// </summary>

public class TriggerForPlayerToDropGMD : MonoBehaviour
{
    public static event Action DroppedGMD;

    [SerializeField, Tooltip("The GMD game object.")]
    private GameObject gmd;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            MakePlayerDropGMD();
        }
    }

    /// <summary>
    /// It is destroyed when the player "drops" it.
    /// </summary>
    private void MakePlayerDropGMD()
    {
        Destroy(gmd.gameObject);

        if (DroppedGMD != null) DroppedGMD.Invoke();
    }
}
