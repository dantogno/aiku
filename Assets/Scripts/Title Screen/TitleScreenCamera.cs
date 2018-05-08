using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the camera used in the title screen
/// </summary>
public class TitleScreenCamera : MonoBehaviour {

    [SerializeField]
    [Tooltip("How much can the player look around by moving the mouse?")]
    private float mouseSensitivity;

    [SerializeField]
    [Tooltip("Smoothing applied to mouse movement")]
    [Range(0, 1)]
    private float smoothing = 0.9f;

    [SerializeField]
    [Tooltip("Animator that runs the glitch effect for transitions between 'shots'")]
    private Animator glitchEffectAnimator;

    /// <summary>
    /// Angle the camera is rotated respective to the Camera Dolly
    /// </summary>
    private float xAngle = 0, yAngle = 0;

    [SerializeField]
    [Tooltip("The camera on the Camera Dolly")]
    private Camera cam;

	// Use this for initialization
	void Start () {
        //player needs the mouse to click on things!
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
	}
	
	// Update is called once per frame
	void Update () {
        MouseLook();
	}

    /// <summary>
    /// Rotate the camera slightly as the player moves the mouse around
    /// </summary>
    private void MouseLook()
    {
        Vector2 mousePositionFromCenter = new Vector2(
            (Input.mousePosition.x - Screen.width / 2) / Screen.width,
            (Input.mousePosition.y - Screen.height / 2) / Screen.height);

        Vector2 targetAngles = mousePositionFromCenter * mouseSensitivity;

        xAngle = Mathf.Lerp(xAngle, targetAngles.x, 1 - smoothing);
        yAngle = Mathf.Lerp(yAngle, targetAngles.y, 1 - smoothing);

        Quaternion rotation = Quaternion.Euler(-yAngle, xAngle, 0);

        cam.transform.localRotation = rotation;
    }

    /// <summary>
    /// Trigger the glitch animation, used to mask the "cut" when the camera jumps to a new location
    /// </summary>
    public void BeginGlitchTransition()
    {
        glitchEffectAnimator.SetTrigger("glitch");
    }
}
