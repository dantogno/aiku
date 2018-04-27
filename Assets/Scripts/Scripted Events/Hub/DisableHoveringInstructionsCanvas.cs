using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class deactivates the hovering instructions canvas that has led the player to the generator shutdown event.
/// The script is applied to the elevator platform trigger, since that is the point where the player will no longer access areas where the canvas will be necessary.
/// </summary>

public class DisableHoveringInstructionsCanvas : MonoBehaviour
{
    [SerializeField, Tooltip("This canvas can be found under the UI root GameObject.")]
    private GameObject hoveringInstructionsCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            hoveringInstructionsCanvas.SetActive(false);
        }
    }
}
