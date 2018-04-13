using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is used to print text to a canvas when the player enters a trigger box collider and remove 
/// the text when the player exits the box collider. It is placed on an empty object with a box collider.
/// </summary>
public class TextBoxTrigger : MonoBehaviour
{

    [Tooltip("This is the canvas that the text is printed to")]
    [SerializeField]
    private Canvas canvas;

    [Tooltip("This is the text that gets printed to the canvas")]
    [SerializeField]
    private Text triggetText;

    void Start()
    {
        triggetText.enabled = false;
    }

    // Enables Canvas and Text when player enters trigger
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            canvas.enabled = true;
            triggetText.enabled = true;      
        }
    }

    // Disables Canvas and Text when player exits trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canvas.enabled = false;
            triggetText.enabled = false;
            triggetText.text = "";
        }
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 10, this.transform.position.z);
    }
}
