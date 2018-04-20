using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is used to print text to a canvas when the player enters a trigger box collider and remove 
/// the text when the player exits the box collider. It is placed on an empty object with a box collider.
/// It must have a canvas and roombaConsole assigned to it.
/// </summary>
public class TextBoxTrigger : MonoBehaviour
{

    [Tooltip("This is the canvas that the text is printed to")]
    [SerializeField]
    private Canvas canvas;

    [Tooltip("Roomba Console that is enabled to swap once box is triggered")]
    [SerializeField]
    private RoombaConsole roombaConsole;

    void Start()
    {
        canvas.enabled = false;
    }

    // Enables Canvas and Text when player enters trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canvas.enabled = true;
            roombaConsole.EnableSwap();
        }
    }
}
