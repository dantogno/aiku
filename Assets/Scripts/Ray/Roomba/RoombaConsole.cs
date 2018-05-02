using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is used to let the player switch between control
/// of 2 different gameObjects that the user controls. These 
/// objects are the roomba and the player. The control switch
/// works by disabling/enabling the player/roomba's respective 
/// cameras, fps controllera, and interaction behaviors.
/// </summary>
public class RoombaConsole : MonoBehaviour, IInteractable
{
    #region Editor fields
    // Editor Fields used for a reference to player and roomba objects
    // in order to swap between them, as well as if the roomba is active

    [Tooltip("Reference to the 'player' game object")]
    [SerializeField]
    GameObject player;

    [Tooltip("Canvas on Player Prefab, child to the scanning camera")]
    [SerializeField]
    Canvas playerCanvas;

    [Tooltip("Reference to the 'roomba' game object")]
    [SerializeField]
    GameObject roomba;

    [Tooltip("Canvas on Roomba Prefab, child to the scanning camera")]
    [SerializeField]
    Canvas roombaCanvas;

    [Tooltip("Roomba Canvas on Roomba Prefab, the canvas not attached to the camera")]
    [SerializeField]
    Canvas swapCanvas;

    [Tooltip("If true, player starts as roomba. If false player starts as player")]
    [SerializeField]
    bool roombaIsActive;
    #endregion


    #region Private fields
    // Private Fields are a camera, 'CustomRigidbodyFPSController', and
    // 'InteractWithSelectedObject' for both roomba and player
    private Camera playerCam;
    private Camera roombaCam;

    private CustomRigidbodyFPSController playerController;
    private CustomRigidbodyFPSController roombaController;

    private InteractWithSelectedObject playerInteractionBehavior;
    private InteractWithSelectedObject roombaInteractionBehavior;

    private bool swapEnabled;
    #endregion

    #region Identifier Strings
    // These might be used for certain scripts (IIdentifiable) in case they are ever needed.
    public string DisplayText { get { return this.gameObject.name; } }
    public string DisplayCommand { get { return "Press Left Mouse Click to interact."; } }
    #endregion

    // Use this for initialization
    void Start()
    {

        #region Defining Private Fields
        // Defines Camera, fpscontroller, and InteractionBehavior for roomba and player objects
        playerCam = player.GetComponentInChildren<Camera>();
        roombaCam = roomba.GetComponentInChildren<Camera>();

        playerController = player.GetComponent<CustomRigidbodyFPSController>();
        roombaController = roomba.GetComponent<CustomRigidbodyFPSController>();

        playerInteractionBehavior = player.GetComponentInChildren<InteractWithSelectedObject>();
        roombaInteractionBehavior = roomba.GetComponentInChildren<InteractWithSelectedObject>();

        playerCanvas = player.GetComponentInChildren<Canvas>();
        roombaCanvas = roomba.GetComponentInChildren<Canvas>();

        swapEnabled = false;
        #endregion

        // Sets the Roomba and player as active or inactive
        if (roombaIsActive)
        {
            ActivateRoomba();
        }
        else // Sets roomba to inactive, player inactive
        {
            DeactivateRoomba();
        }
        swapCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (roombaIsActive)
        {
            //This could be changed to use unity's input handler, but it is currently
            //not for the sake of time and it matches what the claw console does.
            //If changing this, claw console input needs to be changed
            if (Input.GetKey(KeyCode.E))
            {
                if (swapEnabled)
                {
                    SwitchControllers();
                    swapCanvas.enabled = false;
                }
            }
        }

    }

    /// <summary>
    /// When called it will either activate roomba or deactivate roomba
    /// as well as deactivating or activating the player 
    /// </summary>
    private void SwitchControllers() // Triggered once when player interacts with console, or pressing 'e' ass roomba
    {
        if (roombaIsActive) // Switch while roomba is active, deactivates roomba and activates player
        {
            DeactivateRoomba();
        }
        else // Switch while roomba is inactive, activates roomba, deactivates player
        {
            ActivateRoomba();
        }
    }

    /// <summary>
    /// Activates the camera, fpscontroller, and interaction behavior of the roomba
    /// while deactivating those of the player
    /// </summary>
    protected void ActivateRoomba()
    {
        roombaIsActive = true;

        roombaCam.enabled = true;
        playerCam.enabled = false;

        roombaCanvas.enabled = true;
        playerCanvas.enabled = false;

        roombaController.enabled = true;
        playerController.enabled = false;

        roombaInteractionBehavior.enabled = true;
        playerInteractionBehavior.enabled = false;
    }

    /// <summary>
    /// Deactivates the camera, fpscontroller, and interaction behavior of the roomba
    /// while activating those of the player
    /// </summary>
    protected void DeactivateRoomba()
    {
        roombaIsActive = false;

        roombaCam.enabled = false;
        playerCam.enabled = true;

        roombaCanvas.enabled = false;
        playerCanvas.enabled = true;

        roombaController.enabled = false;
        playerController.enabled = true;

        roombaInteractionBehavior.enabled = false;
        playerInteractionBehavior.enabled = true;

    }

    /// <summary>
    /// This allows enables pressing 'e' to swap once the roomba leaves the start room
    /// </summary>
    public void EnableSwap()
    {
        swapEnabled = true;
    }

    /// <summary>
    /// Switches control between the player and the roomba
    /// </summary>
    /// /// <param name="agent">The agent interacting with the object</param>
    public void Interact(GameObject agent) // Calls SwitchControllers()
    {
        SwitchControllers();
    }

}