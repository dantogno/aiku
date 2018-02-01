using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// This class is only used for the player camera's slight headbob reaction to falling from any height.
/// </summary>

public class CustomHeadBob : MonoBehaviour
{
    [Serializable]
    public class CustomCurveControlledBob
    {
        public float HorizontalBobRange = 0f;
        public float VerticalBobRange = 0f;
        public AnimationCurve Bobcurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f),
                                                            new Keyframe(1f, 0f), new Keyframe(1.5f, -1f),
                                                            new Keyframe(2f, 0f)); // sin curve for head bob
        public float VerticaltoHorizontalRatio = 1f;

        private float m_CyclePositionX;
        private float m_CyclePositionY;
        private float m_BobBaseInterval;
        private Vector3 m_OriginalCameraPosition;
        private float m_Time;

        public void Setup(Camera camera, float bobBaseInterval)
        {
            m_BobBaseInterval = bobBaseInterval;
            m_OriginalCameraPosition = camera.transform.localPosition;

            // get the length of the curve in time
            m_Time = Bobcurve[Bobcurve.length - 1].time;
        }

        public Vector3 DoHeadBob(float speed)
        {
            float xPos = m_OriginalCameraPosition.x + (Bobcurve.Evaluate(m_CyclePositionX) * HorizontalBobRange);
            float yPos = m_OriginalCameraPosition.y + (Bobcurve.Evaluate(m_CyclePositionY) * VerticalBobRange);

            m_CyclePositionX += (speed * Time.deltaTime) / m_BobBaseInterval;
            m_CyclePositionY += ((speed * Time.deltaTime) / m_BobBaseInterval) * VerticaltoHorizontalRatio;

            if (m_CyclePositionX > m_Time)
            {
                m_CyclePositionX = m_CyclePositionX - m_Time;
            }
            if (m_CyclePositionY > m_Time)
            {
                m_CyclePositionY = m_CyclePositionY - m_Time;
            }

            return new Vector3(xPos, yPos, 0f);
        }
    }
    
    [Serializable]
    public class CustomLerpControlledBob
    {
        public float BobDuration = .15f;
        public float BobAmount = .2f;

        private float m_Offset = 0f;

        // provides the offset that can be used
        public float Offset()
        {
            return m_Offset;
        }

        public IEnumerator DoBobCycle()
        {
            // make the camera move down slightly
            float t = 0f;
            while (t < BobDuration)
            {
                m_Offset = Mathf.Lerp(0f, BobAmount, t / BobDuration);
                t += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            // make it move back to neutral
            t = 0f;
            while (t < BobDuration)
            {
                m_Offset = Mathf.Lerp(BobAmount, 0f, t / BobDuration);
                t += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            m_Offset = 0f;
        }
    }

    public Camera Camera;
    public CustomCurveControlledBob motionBob = new CustomCurveControlledBob();
    public CustomLerpControlledBob jumpAndLandingBob = new CustomLerpControlledBob();
    public CustomRigidbodyFPSController rigidbodyFirstPersonController;
    public float StrideInterval = 4;
    [Range(0f, 1f)] public float RunningStrideLengthen = .722f;
    
    private bool m_PreviouslyGrounded;
    private Vector3 m_OriginalCameraPosition;

    private void Start()
    {
        if (Camera == null) Camera = GetComponent<Camera>();
        if (rigidbodyFirstPersonController == null)
            rigidbodyFirstPersonController = transform.root.GetComponentInChildren<CustomRigidbodyFPSController>();
        motionBob.Setup(Camera, StrideInterval);
        m_OriginalCameraPosition = Camera.transform.localPosition;
    }


    private void Update()
    {
        UpdateHeadBob();
    }

    private void UpdateHeadBob()
    {
        Vector3 newCameraPosition;
        if (rigidbodyFirstPersonController.Velocity.magnitude > 0 && rigidbodyFirstPersonController.Grounded)
        {
            Camera.transform.localPosition = motionBob.DoHeadBob(rigidbodyFirstPersonController.Velocity.magnitude);
            newCameraPosition = Camera.transform.localPosition;
            newCameraPosition.y = Camera.transform.localPosition.y - jumpAndLandingBob.Offset();
        }
        else
        {
            newCameraPosition = Camera.transform.localPosition;
            newCameraPosition.y = m_OriginalCameraPosition.y - jumpAndLandingBob.Offset();
        }
        Camera.transform.localPosition = newCameraPosition;

        if (!m_PreviouslyGrounded && rigidbodyFirstPersonController.Grounded)
        {
            StartCoroutine(jumpAndLandingBob.DoBobCycle());
        }

        m_PreviouslyGrounded = rigidbodyFirstPersonController.Grounded;
    }
}
