using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Triggers the VO that plays once the player has picked up the GMD
/// </summary>
public class RayVoTrigger : MonoBehaviour
{
	private void OnEnable()
	{
		RotateRings.RotatedRings += TriggerPickupVO;
	}

	private void OnDisable()
	{
		RotateRings.RotatedRings -= TriggerPickupVO;
	}

	/// <summary>
	/// Triggers audio, disables script
	/// </summary>
	private void TriggerPickupVO()
	{
		if (GetComponent<RingVO>() != null)
			GetComponent<RingVO>().TriggerVOAudio();
		this.enabled = false;
	}
}
