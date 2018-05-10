using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingDebugger : MonoBehaviour
{
    [SerializeField] private Cryochamber[] cryochambers;
    [SerializeField] private Door[] doors;

    [SerializeField] private Text totalCryochamberPowerText, playerPowerText, totalPowerText, doorPowerText;

    private PowerableObject[] powerables;
    private PowerableObject player;

    private void Awake()
    {
        powerables = FindObjectsOfType<PowerableObject>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PowerableObject>();
    }

    private void Update()
    {
        UpdateCryoPowerText();
        UpdatePlayerPowerText();
        UpdateTotalPowerText();
        UpdateDoorPowerText();
    }

    private void UpdateCryoPowerText()
    {
        int cryoPower = 0;

        foreach(Cryochamber c in cryochambers)
        {
            // print total amount of power in all cryochambers here, every frame
        }
    }

    private void UpdatePlayerPowerText()
    {
        playerPowerText.text = player.CurrentPower.ToString();
    }

    private void UpdateTotalPowerText()
    {
        int totalPower = 0;

        foreach (PowerableObject p in powerables)
        {
            // print total amount of power in level here, every frame
        }
    }

    private void UpdateDoorPowerText()
    {
        int doorPower = 0;

        foreach (Door d in doors)
        {
            // print all kinds of debugging info about doors on the upper deck here, every frame
        }
    }
}
