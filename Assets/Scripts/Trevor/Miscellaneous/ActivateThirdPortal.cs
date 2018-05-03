using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activate third portal after player has teleported 3 times.
/// </summary>
public class ActivateThirdPortal : MonoBehaviour 
{
    [Tooltip("The fuel cleaner object from Trevor's lab")]
    [SerializeField] private GameObject fuelCleaner;

	private int timesTeleported;

	private void OnEnable()
	{
		PortalNew.PlayerTeleported += IncrementTimesTeleported;
		PortalNew.PlayerTeleported += CheckIfPortalShouldActivate;
    }

	private void OnDisable()
	{
		PortalNew.PlayerTeleported -= IncrementTimesTeleported;
        PortalNew.PlayerTeleported -= CheckIfPortalShouldActivate;
	}

	/// <summary>
	/// Increments the times teleported every every time the player teleports.
	/// </summary>
	private void IncrementTimesTeleported()
	{
		timesTeleported++;
		CheckIfPortalShouldActivate ();
	}

	/// <summary>
	/// If player has teleported 3 times, call the ActivatePortal function.
	/// </summary>
	private void CheckIfPortalShouldActivate()
	{
		if (timesTeleported >= 3) 
		{
			ActivatePortal ();
		}
	}

	/// <summary>
	/// Activate necessary components for teleporter to work properly.
	/// </summary>
	private void ActivatePortal()
	{
        PortalNew portal = GetComponent<PortalNew>();
        GlitchValueGenerator glitch = GetComponent<GlitchValueGenerator>();

        if (portal != null)
            portal.enabled = true;
        if (glitch != null)
            glitch.enabled = true;
        if (fuelCleaner != null)
            fuelCleaner.SetActive(true);
	}
}
