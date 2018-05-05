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

    [Tooltip("The sound effect for teleporting.")]
    [SerializeField] private AudioClip teleportSound;

    [Tooltip("The sound effect for the glitch.")]
    [SerializeField] private AudioClip glitchSound;

    [Tooltip("When true, player will be rotated to the values specified by RotateXAxis and RotateYAxis.")]
    [SerializeField] private bool RotatePlayerOnArrival;

    [Tooltip("Specifies the degree value to rotate the player/camera.")]
    [SerializeField] private float RotateXAxis;

    [SerializeField] private float RotateYAxis;

    public static event Action PlayerTeleported;

    private GlitchValueGenerator glitchValueGeneratorScript;
	private CustomRigidbodyFPSController playerController;
	public Camera playerCamera;
	private bool overThreshold;
    private AudioSource portalAudio;
    private bool hasPlayedTeleportSound = false;
    private bool hasPlayedGlitchSound = false;

    void Start()
    { 
		playerController = player.GetComponent<CustomRigidbodyFPSController> ();
        portalAudio = GetComponent<AudioSource>();
        glitchValueGeneratorScript = GetComponent<GlitchValueGenerator>();
        portalAudio.clip = glitchSound;
        portalAudio.loop = true;
	}

    private void Update()
    {
        if (!portalAudio.isPlaying && hasPlayedTeleportSound)
        {
            this.gameObject.SetActive(false);
        }

        ManageGlitchSound();
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
            if (!hasPlayedTeleportSound)
            {
                portalAudio.clip = teleportSound;
                portalAudio.volume = 1;
                portalAudio.loop = false;
                portalAudio.Play();
                hasPlayedTeleportSound = true;
            }
			if (PlayerTeleported != null)
				PlayerTeleported.Invoke ();

            if(RotatePlayerOnArrival)
            {
                playerController.mouseLook.RotatePlayerTo(RotateXAxis, RotateYAxis);
            }
		}
	}

    private void ManageGlitchSound()
    {
        if (glitchValueGeneratorScript.Value > 0 && !hasPlayedTeleportSound)
        {
            portalAudio.volume = glitchValueGeneratorScript.Value;
            if (!hasPlayedGlitchSound)
            {
                portalAudio.Play();
                hasPlayedGlitchSound = true;
            }
        }
        //else if (glitchValueGeneratorScript.Value <= 0)
        //{
        //    portalAudio.Stop();
        //    hasPlayedGlitchSound = false;
        //}
    }
}
