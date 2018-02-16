using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A power plug is a device found on powerable objects capable of interacting with other power exchangers.
/// Unlike the power switch, the plug features the ability to fill or deplete power incrementally.
/// The portable battery is an example of a powerable object that makes use of a plug.
/// </summary>

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]

public class PowerPlug : PowerExchanger
{
    [SerializeField]
    private Transform plugBeingHeldTransform;

    [SerializeField]
    private InteractWithSelectedObject interactScript;

    private Rigidbody myRigidbody;
    private Collider myCollider;
    private bool playerIsHoldingPlug, canBePickedUp = true;

    // Sliders are placeholders until a more appealing visual indicator is submitted.
    private Slider powerSlider;
    private Text powerText;

    protected override void Awake()
    {
        base.Awake();

        powerSlider = transform.parent.GetComponentInChildren<Slider>();
        powerText = transform.parent.GetComponentInChildren<Text>();

        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        UpdatePowerDisplay();
        UpdateCheckIfCanBeDropped();
    }

    private void OnTriggerEnter(Collider other)
    {
        //IAttachable attachPoint = other.GetComponent<IAttachable>();
        
        /*
        if (attachPoint != null)
        {
            attachPoint.Attach(transform);

            PowerExchanger otherPowerExchanger = other.GetComponentInParent<PowerExchanger>();

            if (otherPowerExchanger != null) otherPowerExchanger.Interact(gameObject);

            print("should attach to power switch");
        }
        */
    }

    public override void Interact(GameObject otherObject)
    {
        // Does not call base.Interact(), because this object transfers power via trigger interactions, via the other object's power switch.

        //PortableBattery battery = null;

        //if (connectedPowerable is PortableBattery) battery = (PortableBattery)connectedPowerable;

        //battery.Interact(otherObject);
    }

    protected override void TransferPower(IPowerable otherObject)
    {

    }

    private void GetPickedUp(Transform otherTransform)
    {
        if (canBePickedUp)
        {
            myRigidbody.isKinematic = true;

            myCollider.enabled = false;

            connectedPowerable.transform.SetParent(otherTransform);

            transform.localPosition = plugBeingHeldTransform.position;
            transform.localEulerAngles = plugBeingHeldTransform.eulerAngles;

            interactScript.enabled = false;

            playerIsHoldingPlug = true;
        }
    }

    private void UpdatePowerDisplay()
    {
        powerSlider.value = connectedPowerable.CurrentPower;
        powerText.text = connectedPowerable.CurrentPower.ToString();
    }

    private void UpdateCheckIfCanBeDropped()
    {
        if (playerIsHoldingPlug && Input.GetButtonDown("Interact"))
        {
            playerIsHoldingPlug = false;

            connectedPowerable.transform.SetParent(null);

            interactScript.enabled = true;

            myRigidbody.isKinematic = false;

            myCollider.enabled = true;
        }
    }
}
