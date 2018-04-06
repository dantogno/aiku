using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class displays the player's power units onscreen as a HUD element.
/// It is applied to the Power UI Canvas prefab, which must be a child object of the Player.
/// </summary>

public class PowerUI : MonoBehaviour
{
    [SerializeField, Tooltip("This is how long it takes a power unit to fill onscreen.")]
    private float fillTime = .5f;

    [SerializeField, Tooltip("These are the blue boxes representing the player's power units.")]
    private Image[] powerUnits, emptyUnits, emptyUnits1, emptyUnits2;

    [SerializeField, Tooltip("The scene changer scripts present in the scene. The power UI is changed when the player chooses a crewmember to save.")]
    private SceneChanger[] sceneChangers;

    // The HUD displays the player's power.
    private PowerableObject playerPower;

    // This float is used to detect changes to the player's power in the Update method.
    private float previousPower;

    private void OnEnable()
    {
        EngineSequenceManager.OnShutdown += EnablePowerUI;
        foreach (SceneChanger sc in sceneChangers)
        {
            sc.ChoseACrewmember += DepleteHalfPlayerPower;
        }
    }
    private void OnDisable()
    {
        EngineSequenceManager.OnShutdown -= EnablePowerUI;
        foreach (SceneChanger sc in sceneChangers)
        {
            sc.ChoseACrewmember -= DepleteHalfPlayerPower;
        }
    }

    private void Start()
    {
        // The Power UI Canvas must be a child of the Player.
        playerPower = GetComponentInParent<PowerableObject>();
    }

    private void Update()
    {
        UpdatePowerUI();
    }

    /// <summary>
    /// Check for changes to the player's power level, and fill/deplete power units accordingly.
    /// </summary>
    private void UpdatePowerUI()
    {
        // If the power level has changed since the last frame...
        if (playerPower.CurrentPower != previousPower)
        {
            // Cycle through each power unit...
            for (int i = 0; i < powerUnits.Length; i++)
            {
                // Check the player's power level, and if a power unit is full or empty.
                bool canFill = playerPower.CurrentPower > i && powerUnits[i].fillAmount == 0,
                    canDeplete = playerPower.CurrentPower <= i && powerUnits[i].fillAmount == 1;

                // If the fill/deplete conditions are met, fill or deplete the power unit.
                if (canFill)
                    StartCoroutine(ChangePowerUnitFillAmount(powerUnits[i], true));
                else if (canDeplete)
                    StartCoroutine(ChangePowerUnitFillAmount(powerUnits[i], false));
            }
        }

        // Set the previousPower variable, for checking in the next frame whether the player's power level has changed.
        previousPower = playerPower.CurrentPower;
    }

    /// <summary>
    /// Fill or deplete a power unit's Image component.
    /// </summary>
    /// <param name="powerUnit"></param>
    /// <param name="empty"></param>
    /// <returns></returns>
    private IEnumerator ChangePowerUnitFillAmount(Image powerUnit, bool empty)
    {
        // Image.fillAmount is a float between 0 and 1, where 0 is empty and 1 is full.
        float originalvalue = empty ? 0 : 1,
            targetValue = empty ? 1 : 0;
        
        float elapsedTime = 0;
        while (elapsedTime < fillTime)
        {
            // Change the fill amount of the power unit's Image with Lerp.
            powerUnit.fillAmount = Mathf.Lerp(originalvalue, targetValue, elapsedTime / fillTime);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        // Make sure the Image is completely full or completely empty.
        powerUnit.fillAmount = targetValue;
    }

    /// <summary>
    /// Disable all power images.
    /// </summary>
    public void DisablePowerUI()
    {
        foreach (Image i in GetComponentsInChildren<Image>())
            i.enabled = false;
    }

    /// <summary>
    /// Enable all power images. This happens when the generator goes offline and the ship switches to emergency power.
    /// </summary>
    private void EnablePowerUI()
    {
        foreach (Image i in GetComponentsInChildren<Image>())
            i.enabled = true;
    }

    /// <summary>
    /// When the player transfers their power to a crewmember, the power units change, even if their power is empty.
    /// </summary>
    /// <param name="s"></param>
    private void DepleteHalfPlayerPower(string s)
    {
        if (emptyUnits[0].fillAmount > 0)
        {
            for (int i = 0; i < emptyUnits1.Length; i++)
            {
                StartCoroutine(ChangePowerUnitFillAmount(emptyUnits1[i], false));
            }
        }
        else
        {
            for (int i = 0; i < emptyUnits2.Length; i++)
            {
                StartCoroutine(ChangePowerUnitFillAmount(emptyUnits2[i], false));
            }
        }
    }
}
