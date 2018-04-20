using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalNew : MonoBehaviour
{
    [Tooltip("The player game object.")]
    [SerializeField] private GameObject player;

    [Tooltip("This is what the player will teleport too. Can use any empty game object.")]
    [SerializeField] private GameObject portalBuddy;

    [Tooltip("Glitchy effect script that is attached to the player camera.")]
    [SerializeField] private GlitchyEffect glitchyEffectScript;

    [Tooltip("Angle that the player will face when teleported. Rotates on Y axis.")]
    [Range(0.0f, 180.0f)] [SerializeField] private float lookAngle;

    [Tooltip("When true, player will be rotated to the values specified by RotateXAxis and RotateYAxis.")]
    [SerializeField] private bool RotatePlayerOnArrival;

    [Tooltip("Specifies the degree value to rotate the player/camera.")]
    [SerializeField] private float RotateXAxis, RotateYAxis;

	public static event Action PlayerTeleported;

	private CustomRigidbodyFPSController playerController;
	public Camera playerCamera;
	private bool overThreshold;
    private AudioSource portalAudio;
    private bool hasPlayedAudio = false;

    void Start()
	{
		playerController = player.GetComponent<CustomRigidbodyFPSController> ();
        portalAudio = GetComponent<AudioSource>();
	}

    private void Update()
    {
        if (!portalAudio.isPlaying && hasPlayedAudio)
        {
            this.gameObject.SetActive(false);
        }
    }

    void OnEnable()
	{
		Scope.ScopedIn += Teleport;
	}

	void OnDisable()
	{
		Scope.ScopedIn -= Teleport;
	}
    
    /// <summary>
    /// Teleports the player to a location specified by Portal Buddy.
    /// </summary>
    /// <param name="i"></param>
	void Teleport(int i)
	{
        overThreshold = glitchyEffectScript.OverThreshold;

        if (overThreshold) 
		{
			player.transform.position = portalBuddy.transform.position;
            glitchyEffectScript.OverThreshold = false;
            if (!portalAudio.isPlaying && !hasPlayedAudio)
            {
                portalAudio.Play();
                hasPlayedAudio = true;
            }
			if (PlayerTeleported != null)
				PlayerTeleported.Invoke ();

            if(RotatePlayerOnArrival)
            {
                playerController.mouseLook.RotatePlayerTo(RotateXAxis, RotateYAxis);
            }
		}
	}
}
