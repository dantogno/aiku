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
    }

    private void Update()
    {
        UpdatePowerDisplay();
    }

    protected override void TransferPower(IPowerable otherObject)
    {

    }

    private void UpdatePowerDisplay()
    {
        powerSlider.value = connectedPowerable.CurrentPower;
        powerText.text = connectedPowerable.CurrentPower.ToString();
    }
}
