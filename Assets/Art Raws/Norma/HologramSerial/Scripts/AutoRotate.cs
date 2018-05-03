using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{

	[SerializeField]
	[Tooltip("The speed of the animation")]
    private float speed = 5.0f;

	[SerializeField]
	[Tooltip("Shoud it rotate on X-Axis")]
    private bool rotateOnX = true;

	[SerializeField]
	[Tooltip("Shoud it rotate on Y-Axis")]
    private bool rotateOnY = true;

	[SerializeField]
	[Tooltip("Shoud it rotate on Z-Axis")]
	private bool rotateOnZ = true;
	
	void Update ()
    {
        Vector3 rotFactor = Vector3.one * speed;

        if (!rotateOnX) rotFactor.x = 0;
        if (!rotateOnY) rotFactor.y = 0;
        if (!rotateOnZ) rotFactor.z = 0;

        transform.Rotate(
            rotFactor * Time.deltaTime
       );
    }
}
