using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerDisplayPlaceholder : PowerExchanger
{
    // Sliders are placeholders until a more appealing visual indicator is submitted.
    private Slider powerSlider;
    private Text powerText;

    protected override void Awake()
    {
        base.Awake();

        powerSlider = GetComponentInChildren<Slider>();
        powerText = GetComponentInChildren<Text>();
        powerSlider.gameObject.SetActive(false);
        powerText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        EngineSequenceManager.OnShutdown += ActivatePowerDisplay;
    }
    private void OnDisable()
    {
        EngineSequenceManager.OnShutdown -= ActivatePowerDisplay;
    }

    private void Update()
    {
        UpdatePowerDisplay();
    }

    protected override void TransferPower(IPowerable otherObject)
    {

    }

    private void ActivatePowerDisplay()
    {
        powerSlider.gameObject.SetActive(true);
        powerText.gameObject.SetActive(true);
    }

    private void UpdatePowerDisplay()
    {
        powerSlider.value = connectedPowerable.CurrentPower;
        powerText.text = connectedPowerable.CurrentPower.ToString();
    }
}
