using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



/// <summary>
/// GMD scope funtionality. Should be attached to GMD game object.
/// </summary>
public class Scope : MonoBehaviour
{
	[Tooltip("Lens mask game object. Should be childed to player camera and disabled.")]
	[SerializeField] private GameObject lensMask;

	[Tooltip("Overlay game object. Parent object of tint and overlay image. Should be childed to player camera and disabled.")]
	[SerializeField] private GameObject overlay;

	[Tooltip("Parent object of the 'Body' and 'End Piece' objects of the GMD.")]
	[SerializeField] private GameObject gmdModel;

	[Tooltip("Render camera for GMD so that it draws last. Childed to Renderer Cameras game object.")]
	[SerializeField] private GameObject gmdCameraObject;

	[Tooltip("Animator component of the GMD.")]
	[SerializeField] private Animator zoomAnimator;

	[Tooltip("CustomRigidbodyFPSController component of the player")]
	[SerializeField] private CustomRigidbodyFPSController character;

    //Currently used by another script. Will be made private eventually.
    public bool isEquipped;

	public static event Action<int> ScopedIn;

	private GameObject[] metalObjects;
	private GameObject[] grappleObjects;
	private GameObject[] goodOre;
    private bool hasWaitedForAnimation;
	private bool hasSentEvent = false;
	private bool hasFirstEventBeenSent = false;

	Material metalObjectMaterial;
	Material grappleObjectMaterial;
	Material goodOreMaterial;

	private void Start ()
	{
        isEquipped = false;
		overlay.SetActive(false);
		gmdModel.SetActive(true);
    }

	private void Update ()
	{
		if (Input.GetButtonDown("Scope") && !isEquipped)
		{
            isEquipped = true;
            StartCoroutine(WaitForScopeAnimations());
        }

		else if (Input.GetButtonDown("Scope") && isEquipped)
		{
            isEquipped = false;
            StartCoroutine(WaitForScopeAnimations());
        }

        Equip();
        Unequip();
		SendEvent ();
        SetEmission();
	}

	/// <summary>
	/// Starts animation for scoping in. Disables gmdCameraObject. Waits for animation to finish then turns on lensMask and overlay, and turns off the gmd model.
	/// </summary>
	private void Equip()
	{
        if(isEquipped)
        {
            zoomAnimator.SetBool("UsingScope", true);
			gmdCameraObject.SetActive(false);

			SlowDownPlayer ();

            if (hasWaitedForAnimation)
            {
                isEquipped = true;
                lensMask.SetActive(true);
				overlay.SetActive(true);
				gmdModel.SetActive(false);
            }
        }
	}

	/// <summary>
	/// Starts animation for scoping out. Turns off lensMask and overlay, and turns on gmd model. Waits for animation then turns the gmdCameraObject back on.
	/// </summary>
	private void Unequip()
	{
        if(!isEquipped)
        {
            zoomAnimator.SetBool("UsingScope", false);
            isEquipped = false;
            lensMask.SetActive(false);
			overlay.SetActive(false);
			gmdModel.SetActive(true);

			ResetPlayerMovementSpeed ();

            if (hasWaitedForAnimation)
            {
				gmdCameraObject.SetActive(true);
            }
        }
		
	}

	/// <summary>
	/// Sends an event to tell others when the player is scoped in.
	/// </summary>
	private void SendEvent()
	{
		if (isEquipped && hasWaitedForAnimation && !hasSentEvent) 
		{
			if (!hasFirstEventBeenSent) 
			{
				gmdModel.GetComponent<VOAudio> ().TriggerVOAudio ();
				hasFirstEventBeenSent = true;
			}

			if (ScopedIn != null) 
			{
				ScopedIn.Invoke(0);
				hasSentEvent = true;
			}
		} 
		else if (!isEquipped && hasWaitedForAnimation) 
		{
			hasSentEvent = false;
		}
	}

	/// <summary>
	/// Sets the emission color and intensity of object tagged either "Metal Object", "Grapple", or "Good Ore" when scoped in or out.
	/// </summary>
    private void SetEmission()
    {
        if (isEquipped && hasWaitedForAnimation)
        {
            metalObjects = GameObject.FindGameObjectsWithTag("Metal Object");
            grappleObjects = GameObject.FindGameObjectsWithTag("Grapple");
            goodOre = GameObject.FindGameObjectsWithTag("Good Ore");



            foreach (GameObject item in metalObjects)
            {
                metalObjectMaterial = item.GetComponent<Renderer>().material;
                metalObjectMaterial.EnableKeyword("_EMISSION");
                metalObjectMaterial.SetColor("_EmissionColor", Color.red);
            }
            foreach (GameObject item in grappleObjects)
            {
                grappleObjectMaterial = item.GetComponent<Renderer>().material;
                grappleObjectMaterial.EnableKeyword("_EMISSION");
                grappleObjectMaterial.SetColor("_EmissionColor", Color.blue);
            }
            foreach (GameObject item in goodOre)
            {
                goodOreMaterial = item.GetComponent<Renderer>().material;
                goodOreMaterial.EnableKeyword("_EMISSION");
                goodOreMaterial.SetColor("_EmissionColor", Color.yellow);
            }
        }
        else if (!isEquipped)
        {
            metalObjects = GameObject.FindGameObjectsWithTag("Metal Object");
            grappleObjects = GameObject.FindGameObjectsWithTag("Grapple");
            goodOre = GameObject.FindGameObjectsWithTag("Good Ore");



			foreach (GameObject item in metalObjects)
            {
                metalObjectMaterial = item.GetComponent<Renderer>().material;
                metalObjectMaterial.EnableKeyword("_EMISSION");
                metalObjectMaterial.SetColor("_EmissionColor", new Color(0, 0, 0, 0.000001f));
            }
            foreach (GameObject item in grappleObjects)
            {
                grappleObjectMaterial = item.GetComponent<Renderer>().material;
                grappleObjectMaterial.EnableKeyword("_EMISSION");
                grappleObjectMaterial.SetColor("_EmissionColor", new Color(0, 0, 0, 0.000001f));
            }
            foreach (GameObject item in goodOre)
            {
                goodOreMaterial = item.GetComponent<Renderer>().material;
                goodOreMaterial.EnableKeyword("_EMISSION");
                goodOreMaterial.SetColor("_EmissionColor", new Color(0, 0, 0, 0.000001f));
            }
        }
    }

	/// <summary>
	/// Slows down player
	/// </summary>
	private void SlowDownPlayer()
	{
		character.movementSettings.ForwardSpeed = 0.7f;
		character.movementSettings.BackwardSpeed = 0.5f;
		character.movementSettings.StrafeSpeed = 0.5f;
	}

	/// <summary>
	/// Resets the player movement speed.
	/// </summary>
	private void ResetPlayerMovementSpeed()
	{
		character.movementSettings.ForwardSpeed = 5f;
		character.movementSettings.BackwardSpeed = 3.5f;
		character.movementSettings.StrafeSpeed = 3.5f;
	}

	/// <summary>
	/// Waits for scope animations.
	/// </summary>
	/// <returns>The for scope animations.</returns>
    private IEnumerator WaitForScopeAnimations()
    {
        hasWaitedForAnimation = false;
        yield return new WaitForSeconds(zoomAnimator.GetCurrentAnimatorStateInfo(0).length);
        hasWaitedForAnimation = true;
    }
}
