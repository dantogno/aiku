using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Our elevator's buttons. 
///  IInteractables that, when clicked, alerts the elevator which one has been clicked in a psudo-mediator pattern fashion. 
/// </summary>
public class RayElevatorButtonScript : MonoBehaviour , IInteractable
{
    [SerializeField]
    private ElevatorScript elevatorScript;

    Light buttonLight;

    [SerializeField]
    private bool isButtonPressed;

    [SerializeField]
    private Renderer buttonRenderer;

    [SerializeField]
    private Material darkMaterial;

    [SerializeField]
    private Material lightMaterial;

    [SerializeField]
    private float timeToStayLit;

    void Start()
    {
        elevatorScript = FindObjectOfType<ElevatorScript>();
        buttonRenderer = gameObject.GetComponent<Renderer>();

        buttonRenderer.material = darkMaterial;
        buttonLight = GetComponent<Light>();
        buttonLight.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {

        if (timeToStayLit <= 0f)
        {
            isButtonPressed = false;
            TurnLightOff();
        }
    }

    public void Interact(GameObject agent)
    {
        //TODO, add click sound;
        timeToStayLit = 0.5f;
        if (this.gameObject.name == "Elevator Button Up")
        {
            elevatorScript.Interact(this.gameObject);
        }
        if (this.gameObject.name == "Elevator Button Down")
        {
            elevatorScript.Interact(this.gameObject);
        }
    }

    private void TurnLightOn()
    {
        buttonRenderer.material = lightMaterial;
        timeToStayLit -= Time.deltaTime;
    }

    private void TurnLightOff()
    {
        buttonRenderer.material = darkMaterial;
    }

}
