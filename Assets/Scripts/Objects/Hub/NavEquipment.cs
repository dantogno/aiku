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
    private List<Material> computerMaterials;

    private void Awake()
    {
        myRenderer = GetComponent<MeshRenderer>();
        myPowerable = GetComponent<PowerableObject>();

        computerMaterials = new List<Material>();
        
        foreach (Material m in myRenderer.materials)
        {
            computerMaterials.Add(m);
        }
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
        foreach (Material m in computerMaterials) StartCoroutine(FadeScreens(m, false));
    }

    /// <summary>
    /// Turn on the screens when the powerable powers up.
    /// </summary>
    private void TurnOnScreens()
    {
        foreach (Material m in computerMaterials) StartCoroutine(FadeScreens(m, true));
    }

    /// <summary>
    /// Gradually change the color of the screens.
    /// </summary>
    private IEnumerator FadeScreens(Material m, bool fadeIn)
    {
        Color targetColor = fadeIn ? Color.white : Color.black,
            originalColor = m.color,
            currentColor = originalColor;

        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            currentColor = Color.Lerp(originalColor, targetColor, elapsedTime / fadeTime);
            m.SetColor("_EmissionColor", currentColor);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        m.SetColor("_EmissionColor", targetColor);
    }
}
