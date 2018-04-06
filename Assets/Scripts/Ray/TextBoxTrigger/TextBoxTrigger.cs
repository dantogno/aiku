using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxTrigger : MonoBehaviour
{
    /// <summary>
    /// This script is used to print text to a canvas when the player enters a trigger box collider and remove 
    /// the text when the player exits the box collider. It is placed on an empty object with a box collider.
    /// </summary>

    [Tooltip("This is the canvas that the text is printed to")]
    [SerializeField]
    public Canvas Canvas;

    [Tooltip("This is the text that gets printed to the canvas")]
    [SerializeField]
    public Text TriggerText;
   
    // This is the player object that triggers the text
    private CustomRigidbodyFPSController customRigidBodyFPSController;

    void Start()
    {
        TriggerText.enabled = false;
    }

    // Enables Canvas and Text when player enters trigger
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Canvas.enabled = true;
            TriggerText.enabled = true;      
        }
    }

    // Disables Canvas and Text when player exits trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Canvas.enabled = false;
            TriggerText.enabled = false;
            TriggerText.text = "";
        }
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 10, this.transform.position.z);
    }
}
