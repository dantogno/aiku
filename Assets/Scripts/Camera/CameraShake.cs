using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class shakes the camera when the generator explodes.
/// The script is applied to the main camera.
/// </summary>

[RequireComponent(typeof(Camera))]
public class CameraShake : MonoBehaviour
{
    [SerializeField, Tooltip("Generator explosion shake variable.")]
    private float explosionShakeMagnitude = 1, explosionShakeTimer = 2;

    private void OnEnable()
    {
        Generator.Exploded += OnGeneratorExploded;
    }
    private void OnDisable()
    {
        Generator.Exploded -= OnGeneratorExploded;
    }

    private void OnGeneratorExploded()
    {
        StartCoroutine(Shake());
    }

    /// <summary>
    /// Shake the camera.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Shake()
    {
        Vector3 originalCamPos = Camera.main.transform.localPosition;

        #region Shake camera by randomly moving camera around inside a sphere within a time limit.

        float elapsedTime = 0;
        while (elapsedTime < explosionShakeTimer)
        {
            Camera.main.transform.localPosition = new Vector3
                (originalCamPos.x + Random.insideUnitSphere.x * explosionShakeMagnitude,
                originalCamPos.y + Random.insideUnitSphere.y * explosionShakeMagnitude,
                Camera.main.transform.localPosition.z);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        #endregion

        // Reset camera position back to original position.
        Camera.main.transform.localPosition = originalCamPos;
    }
}
