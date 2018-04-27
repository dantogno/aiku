using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class allows the screens of the nav equipment to darken when power is extracted.
/// The script is applied to the Nav Equipment GameObject.
/// </summary>

public class NavEquipment : MonoBehaviour
{
    [SerializeField, Tooltip("How long the screens take to fade.")]
    private float fadeTime = 1;

    private MeshRenderer myRenderer;
    private PowerableObject myPowerable;
    private Material computerMaterial;

    private void Awake()
    {
        myRenderer = GetComponent<MeshRenderer>();
        myPowerable = GetComponent<PowerableObject>();

        // The third material in this mesh renderer's material array is the one for the computer monitors.
        computerMaterial = myRenderer.materials[2];
    }

    private void OnEnable()
    {
        myPowerable.OnPoweredOff += TurnOffScreens;
        myPowerable.OnPoweredOn += TurnOnScreens;
    }
    private void OnDisable()
    {
        myPowerable.OnPoweredOff -= TurnOffScreens;
        myPowerable.OnPoweredOn -= TurnOnScreens;
    }

    /// <summary>
    /// Turn off the screens when the powerable powers down.
    /// </summary>
    private void TurnOffScreens()
    {
        StartCoroutine(FadeScreens(false));
    }

    /// <summary>
    /// Turn on the screens when the powerable powers up.
    /// </summary>
    private void TurnOnScreens()
    {
        StartCoroutine(FadeScreens(true));
    }

    /// <summary>
    /// Gradually change the color of the screens.
    /// </summary>
    private IEnumerator FadeScreens(bool fadeIn)
    {
        Color targetColor = fadeIn ? Color.white : Color.black,
            originalColor = computerMaterial.color;

        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            computerMaterial.color = Color.Lerp(originalColor, targetColor, elapsedTime / fadeTime);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        computerMaterial.color = targetColor;
    }
}
