using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Generates a value between 0 and 1 for determining the intensity of a glitch effect.
/// This number is generated based on a combination of distance and angle from this object.
/// </summary>
public class GlitchValueGenerator : MonoBehaviour {

    [SerializeField] GameObject Player;
    [SerializeField] float MaxDistance;
    [Range(0.0f, 180.0f)] [SerializeField] float MaxAngle;

    public static Action<float> ValueAboveZero;

    public float Value
    {
        get
        {
            return CalculateValue(DistanceToGlitch(), AngleToGlitch());
        }

        private set { }
    }

    private Camera plyrCamera;

    /// <summary>
    /// Returns the angle between this and the forward vector of the Player.
    /// </summary>
    /// <returns></returns>
    float AngleToGlitch()
    {
        float a;

        a = Vector3.Angle((this.transform.position - plyrCamera.transform.position), plyrCamera.transform.forward);

#if UNITY_EDITOR
        Debug.Log(a + " degrees");
#endif

        return a;
    }

    /// <summary>
    /// Returns the distance from this to Player.
    /// </summary>
    /// <returns></returns>
    float DistanceToGlitch()
    {
        float d;

        d = Vector3.Distance(this.transform.position, Player.transform.position);

#if UNITY_EDITOR
        Debug.Log(d + " units away");
#endif

        return d;
    }

    /// <summary>
    /// Returns a weighted value between 0 and 1.
    /// The value created is used to determine Glitch Intensity.
    /// </summary>
    /// <param name="distance"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    float CalculateValue(float distance, float angle)
    {
        float n;

        float normDistance = NormalizeDistance(distance);
        float normAngle = NormalizeAngle(angle);

        //n = (normDistance + normAngle) / 2;     //Take the mean value of the distance and angle       //Alternative method of generating the combined output
        n = normDistance * normAngle;

        return n;
    }

    /// <summary>
    /// Takes dist and returns a percentage value between 0 and 1, 0 being the Max Distance.
    /// </summary>
    /// <param name="dist"></param>
    /// <returns></returns>
    float NormalizeDistance(float dist)
    {
        float d;

        if (dist >= MaxDistance)
        {
            d = 0;
        }
        else if (dist <= 0)
        {
            d = 1;
        }
        else
        {
            d = 1 - (dist / MaxDistance);
        }

#if UNITY_EDITOR
        Debug.Log(d + " Distance");
#endif

        return d;
    }

    /// <summary>
    /// Takes angle and returns a percentage value between 0 and 1, 0 being the Max Angle.
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    float NormalizeAngle(float angle)
    {
        float a;

        if (angle >= MaxAngle)
        {
            a = 0;
        }
        else if (angle <= 0)
        {
            a = 1;
        }
        else
        {
            a = 1 - (angle / MaxAngle);
        }

#if UNITY_EDITOR
        Debug.Log(a + " Angle");
#endif

        return a;
    }

    private float i;

    private void Update()
    {
        i = Value;

        if(i > 0)
        {
            ValueAboveZero(i);
        }

#if UNITY_EDITOR
        Debug.Log(i);
#endif
    }

    private void Awake()
    {
        plyrCamera = Player.GetComponentInChildren<Camera>();
    }
}