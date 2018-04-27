using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class causes a light to flash when the pipe in the engine room bursts.
/// The script is applied to a light near the fuel station.
/// </summary>

public class FuelManifoldLight : MonoBehaviour
{
    [SerializeField, Tooltip("After this task is complete, the light flashes once.")]
    private Task fuelCheckTask;

    [SerializeField, Tooltip("How long it takes for the light to fade in or out")]
    private float fadeTime = .1f;

    [SerializeField, Tooltip("Desired intensity of the light.")]
    private float targetIntensity = 3;

    private void OnEnable()
    {
        fuelCheckTask.OnTaskCompleted += CallFlashLightCoroutine;
    }
    private void OnDisable()
    {
        fuelCheckTask.OnTaskCompleted -= CallFlashLightCoroutine;
    }

    /// <summary>
    /// When the pipe bursts, the light flashes.
    /// </summary>
    private void CallFlashLightCoroutine()
    {
        StartCoroutine(FlashLight());
    }

    /// <summary>
    /// Fade the light in, then out.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FlashLight()
    {
        Light myLight = GetComponent<Light>();

        if (myLight != null)
        {
            #region Fade in.

            float elapsedTime = 0;
            while (elapsedTime < fadeTime)
            {
                myLight.intensity = Mathf.Lerp(0, targetIntensity, elapsedTime / fadeTime);

                yield return new WaitForEndOfFrame();
                elapsedTime += Time.deltaTime;
            }

            #endregion

            myLight.intensity = targetIntensity;

            #region Fade out.

            elapsedTime = 0;
            while (elapsedTime < fadeTime)
            {
                myLight.intensity = Mathf.Lerp(targetIntensity, 0, elapsedTime / fadeTime);

                yield return new WaitForEndOfFrame();
                elapsedTime += Time.deltaTime;
            }

            #endregion

            myLight.intensity = 0;
        }

    }
}
