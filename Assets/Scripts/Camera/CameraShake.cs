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

    [SerializeField, Tooltip("When this task is complete, the camera shakes a little bit.")]
    private Task pipeBurstTask;

    private void OnEnable()
    {
        Generator.Exploded += OnGeneratorExploded;
        pipeBurstTask.OnTaskCompleted += OnPipeBurst;
    }
    private void OnDisable()
    {
        Generator.Exploded -= OnGeneratorExploded;
        pipeBurstTask.OnTaskCompleted -= OnPipeBurst;
    }

    private void OnGeneratorExploded()
    {
        StartCoroutine(GeneratorShake());
    }

    private void OnPipeBurst()
    {
        StartCoroutine(PipeShake());
    }

    /// <summary>
    /// Shake the camera when the generator explodes.
    /// </summary>
    private IEnumerator GeneratorShake()
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

    /// <summary>
    /// Shake the camera when the pipe bursts.
    /// </summary>
    private IEnumerator PipeShake()
    {
        // The pipe bursting shouldn't be a big shake, so we split the generator shake values in half.
        explosionShakeTimer /= 2;
        explosionShakeMagnitude /= 2;

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

        // We return the explosion variables to their original values.
        explosionShakeTimer *= 2;
        explosionShakeMagnitude *= 2;
    }
}
