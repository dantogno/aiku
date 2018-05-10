using System;
using UnityEngine;

/// <summary>
/// This class controls the player character's movement and camera rotation.
/// Its original logic was written by whoever wrote the Unity Standard Assets rigidbody controller.
/// </summary>

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class CustomRigidbodyFPSController : MonoBehaviour
{
    [Serializable]
    public class MovementSettings
    {
        public float ForwardSpeed = 5.0f;   // Speed when walking forward
        public float BackwardSpeed = 3.0f;  // Speed when walking backwards
        public float StrafeSpeed = 4.0f;    // Speed when walking sideways

        // Robert: look here
        public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));

        [HideInInspector] public float CurrentTargetSpeed = 5.0f;
        
        public void UpdateDesiredTargetSpeed(Vector2 input)
        {
            if (input == Vector2.zero) return;
            if (input.x > 0 || input.x < 0)
            {
                //strafe
                CurrentTargetSpeed = StrafeSpeed;
            }
            if (input.y < 0)
            {
                //backwards
                CurrentTargetSpeed = BackwardSpeed;
            }
            if (input.y > 0)
            {
                //forwards
                //handled last as if strafing and moving forward at the same time forwards speed should take precedence
                CurrentTargetSpeed = ForwardSpeed;
            }
        }
    }

    [Serializable]
    public class CustomMouseLook
    {
        public float XSensitivity = 2f;
        public float YSensitivity = 1.5f;
        public bool clampVerticalRotation = true;
        public float MinimumX = -75F;
        public float MaximumX = 75F;
        public bool smooth = true;
        public float smoothTime = 50f;

        private Quaternion m_CharacterTargetRot;
        private Quaternion m_CameraTargetRot;

        public void Init(Transform character, Transform camera)
        {
            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;
        }

        public void LookRotation(Transform character, Transform camera)
        {
            float yRot = Input.GetAxis("Mouse X") * XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation)
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

            if (smooth)
            {
                character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }
        }

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

        /// <summary>
        /// Rotates the camera and player character around the X-Axis (xRot) and Y-Axis (yRot)
        /// </summary>
        /// <param name="xRot"></param>
        /// <param name="yRot"></param>
        public void RotatePlayerTo(float xRot, float yRot)
        {
            m_CharacterTargetRot = Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot = Quaternion.Euler(-xRot, 0f, 0f);
        }
    }

    [Serializable]
    public class AdvancedSettings
    {
        public float groundCheckDistance = 0.1f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
        [Tooltip("the comment I found next to this variable says 'stops the character' (???)")]
        public float stickToGroundHelperDistance = 0.5f; // stops the character
        [Tooltip("set it to 0.1 or more if you get stuck in wall")]
        public float shellOffset = 0.1f; //reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
    }

    public Camera cam;
    public MovementSettings movementSettings = new MovementSettings();
    public CustomMouseLook mouseLook = new CustomMouseLook();
    public AdvancedSettings advancedSettings = new AdvancedSettings();
    public PhysicMaterial movingPhysicMaterial, stoppedPhysicMaterial;

    private Rigidbody m_RigidBody;
    private CapsuleCollider m_Capsule;
    private Vector3 m_GroundContactNormal;
    private Vector2 input;  // DW
    private bool m_PreviouslyGrounded, m_IsGrounded;

    public Vector3 Velocity
    {
        get
        {
            // TODO: This fixes the unhandled null
            // not sure if it fixes it well...
            // needed to fix to play with debugger on!
            if (m_RigidBody == null)
                return Vector3.zero;
            else
                return m_RigidBody.velocity;
        }
    }

    public bool Grounded
    {
        get { return m_IsGrounded; }
    }

    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        if (cam == null) cam = GetComponentInChildren<Camera>();
        mouseLook.Init(transform, cam.transform);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        RotateView();
        GroundCheck();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
        UpdateFriction();
    }

    private void UpdateMovement()
    {
        input = GetInput();

        if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && m_IsGrounded)
        {
            // always move along the camera forward as it is the direction that it being aimed at
            // ^This has been modified so that forward is now the direction of the player, and not the camera.
            Vector3 desiredMove = transform.forward * input.y + transform.right * input.x;
            desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;

            desiredMove.x = desiredMove.x * movementSettings.CurrentTargetSpeed;
            desiredMove.z = desiredMove.z * movementSettings.CurrentTargetSpeed;
            desiredMove.y = desiredMove.y * movementSettings.CurrentTargetSpeed;

            // Robert: look here
            if (m_RigidBody.velocity.sqrMagnitude <
                (movementSettings.CurrentTargetSpeed * movementSettings.CurrentTargetSpeed))
            {
                m_RigidBody.AddForce(desiredMove * SlopeMultiplier(), ForceMode.Impulse);
            }
        }

        MakeSlightlyMoreComplexMovementAdjustments();
    }

    private void MakeSlightlyMoreComplexMovementAdjustments()
    {
        if (m_IsGrounded)
        {
            m_RigidBody.drag = 5f;

            if (Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && m_RigidBody.velocity.magnitude < 1f)
            {
                m_RigidBody.Sleep();
            }
        }
        else
        {
            m_RigidBody.drag = 0f;
            if (m_PreviouslyGrounded)
            {
                StickToGroundHelper();
            }
        }
    }

    private void UpdateFriction()
    {
        if (Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon)
        {
            m_Capsule.material = movingPhysicMaterial;
        }
        else
        {
            m_Capsule.material = stoppedPhysicMaterial;
        }
    }

    // Robert: look here
    private float SlopeMultiplier()
    {
        float angle = Vector3.Angle(m_GroundContactNormal, Vector3.up);
        return movementSettings.SlopeCurveModifier.Evaluate(angle);
    }

    // this is perplexing
    private void StickToGroundHelper()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset),
            Vector3.down, out hitInfo, ((m_Capsule.height / 2f) - m_Capsule.radius) +
            advancedSettings.stickToGroundHelperDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
            {
                m_RigidBody.velocity = Vector3.ProjectOnPlane(m_RigidBody.velocity, hitInfo.normal);
            }
        }
    }

    private Vector2 GetInput()
    {
        Vector2 input = new Vector2
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical")
        };

        movementSettings.UpdateDesiredTargetSpeed(input);
        return input;
    }

    private void RotateView()
    {
        //avoids the mouse looking if the game is effectively paused
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        // get the rotation before it's changed
        float oldYRotation = transform.eulerAngles.y;

        mouseLook.LookRotation(transform, cam.transform);

        if (m_IsGrounded)
        {
            // Rotate the rigidbody velocity to match the new direction that the character is looking
            Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
            m_RigidBody.velocity = velRotation * m_RigidBody.velocity;
        }
    }

    /// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
    private void GroundCheck()
    {
        m_PreviouslyGrounded = m_IsGrounded;
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                               ((m_Capsule.height / 2f) - m_Capsule.radius) + advancedSettings.groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            m_IsGrounded = true;
            m_GroundContactNormal = hitInfo.normal;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundContactNormal = Vector3.up;
        }
    }
}
