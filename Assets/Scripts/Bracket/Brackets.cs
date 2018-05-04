using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Finds the onscreen boundaries of the current object to interact with, and moves UI "brackets" to select that area
/// </summary>
public class Brackets : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    [Tooltip("Render texture that the scanning camera is rendering to")]
    private RenderTexture renderTexture;
    [SerializeField]
    [Tooltip("Sampling detail (lower is higer res)")]
    [Range(0.001f, 0.05f)]
    private float resolution;
    [SerializeField]
    [Tooltip("UI element containing the brackets")]
    private RectTransform bracketArea;
    [SerializeField]
    private float bracketScreenBuffer;
    [SerializeField]
    [Tooltip("time in seconds brackets will take to move")]
    private float bracketSmoothTiming = 0.1f; 
    [SerializeField]
    [Tooltip("max speed in pixels/sec brackets move")]
    [Range(100, 1000)]
    private float bracketLerpSpeed = 1000;
    [SerializeField]
    [Tooltip("Area to leave brackets when no object is being targeted")]
    private Rect defaultBracketArea;
    [SerializeField]
    [Tooltip("Use the spinning center bracket?")]
    private bool UseCenterBracket = false;
    [SerializeField]
    private Image CenterBracket;   
    [SerializeField]
    float centerBracketRotationSpeed = 1f;
    #endregion

    #region Private Fields
    private Vector2 bracketAnchorMinVelocity = Vector3.zero;
    private Vector2 bracketAnchorMaxVelocity = Vector3.zero;
    // GameObject that has been sent to the shadow realm, so that we can return it when a new target is selected
    private GameObject currentScanningLayerObject = null;
    // Holds a reference to the current object's previous layer (so that it can be restored later)
    private int currentObjectPreviousLayer = 0;

    // Layer that the scanning camera is looking at
    static LayerMask scanningLayer;

    // The render texture is copied into this Texture2D so that we can check individual pixels' alpha
    private Texture2D scanningImage;

    #endregion

    #region Unity Methods
    // Use this for initialization
    void Start()
    {
        scanningLayer = LayerMask.NameToLayer("BracketLayer");
        scanningImage = new Texture2D(renderTexture.width, renderTexture.height);

    }

    // Update is called once per frame
    void Update()
    {
        // Depending on performance impact it may be better to do this every few frames instead of every time, but seems negligible if you set resolution at a decent value 
        

    }
    private void LateUpdate()
    {
        DrawBrackets(FindObjectBounds());
    }

    private void OnEnable()
    {
        DetectInteractableObject.ObjectToInteractWithChanged += ObjectToInteractWithChangedHandler;
    }

    private void OnDisable()
    {
        DetectInteractableObject.ObjectToInteractWithChanged -= ObjectToInteractWithChangedHandler;
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Event handler for DetectInteractableObject.ObjectToInteractWithChanged
    /// </summary>
    /// <param name="objectToInteractWith">This is the GameObject that the player can currently interact with</param>
    private void ObjectToInteractWithChangedHandler(IInteractable objectToInteractWith)
    {
        if (objectToInteractWith == null)
            MoveObjectToScanningLayer(null);
        else
            MoveObjectToScanningLayer(((MonoBehaviour)objectToInteractWith).gameObject);
    }

    /// <summary>
    /// Moves a GameObject to the scanning layer so that it shows up in the renderTexture / scanningImage
    /// </summary>
    /// <param name="objectToMove">The object to put on the scanning layer</param>
    private void MoveObjectToScanningLayer(GameObject objectToMove)
    {
        // If there was a previous object on the scanning layer, return it to its old layer
        if (currentScanningLayerObject != null)
        {
            ChangeLayerRecursive(currentScanningLayerObject, currentObjectPreviousLayer);
        }

        // If there is a new object we want to select, move it to the scanning layer
        if (objectToMove != null)
        {
            // Save a reference to the previous layer
            currentObjectPreviousLayer = objectToMove.layer;
            ChangeLayerRecursive(objectToMove, scanningLayer);
        }
        else
        {
            // Reset the previous layer if no object was passed
            CenterBracketInactive();
            currentObjectPreviousLayer = 0;           
        }

        // Remember the new object on the scanning layer so we can put it back later
        currentScanningLayerObject = objectToMove;
    }

    private void ChangeLayerRecursive(GameObject target, int layer)
    {
        target.layer = layer;
        for(int i = 0; i < target.transform.childCount; i++)
        {
            GameObject child = target.transform.GetChild(i).gameObject;

            //ignore world-space Canvas, all other objects are fine
            Canvas childIsCanvas = child.GetComponent<Canvas>();
            if (childIsCanvas == null)
            {
                ChangeLayerRecursive(child, layer);
            }
        }
    }

    /// <summary>
    /// Check the scanningImage to find out where the object is
    /// </summary>
    /// <returns>Normalised coordinates of the object on the screen</returns>
    private Rect FindObjectBounds()
    {
        // Start with everything off the screen
        float leftBound = 1,
            rightBound = 0,
            lowerBound = 1,
            upperBound = 0;

        // Sample (resolution x resolution) points out of the texture and check if the object is there
        for (float x = 0; x < 1; x += resolution)
        {
            for (float y = 0; y < 1; y += resolution)
            {
                if (PixelIsObject(x, y))
                {
                    // Pick the furthest left, right, up, and down coordinates from samples that "hit" the object
                    leftBound = Mathf.Min(x, leftBound);
                    rightBound = Mathf.Max(x, rightBound);
                    lowerBound = Mathf.Min(y, lowerBound);
                    upperBound = Mathf.Max(y, upperBound);
                    //Activate center bracket
                    if (UseCenterBracket)
                    {
                        RotateCenterBracket();
                        CenterBracketActive();
                    }
                }
            }
        }

        // This Rect surrounds the area where the object is in normalised screen coordinates
        Rect bounds = new Rect(leftBound, lowerBound, rightBound - leftBound, upperBound - lowerBound);
       
        // If we can't see the object or if no object is selected, revert to defaults
        if (bounds.min == Vector2.one && bounds.max == Vector2.zero)
        {
            bounds = defaultBracketArea;
            CenterBracketInactive();
        }

        return bounds;
    }

    /// <summary>
    /// Moves the bracket's anchors to match the given rect
    /// </summary>
    /// <param name="rect">The area surrounding the current object</param>
    private void DrawBrackets(Rect rect)
    {
        bracketArea.anchorMin = Vector2.SmoothDamp(bracketArea.anchorMin, new Vector2(rect.min.x - bracketScreenBuffer, rect.min.y - bracketScreenBuffer), ref bracketAnchorMinVelocity, bracketSmoothTiming, bracketLerpSpeed, Time.deltaTime);

        bracketArea.anchorMax = Vector2.SmoothDamp(bracketArea.anchorMax, new Vector2(rect.max.x + bracketScreenBuffer, rect.max.y + bracketScreenBuffer), ref bracketAnchorMaxVelocity, bracketSmoothTiming, bracketLerpSpeed, Time.deltaTime);
    }

    /// <summary>
    /// Checks a pixel to see if the object is there
    /// </summary>
    /// <param name="x">Normalised x coord in the image to check</param>
    /// <param name="y">Normalised y coord in the image to check</param>
    /// <returns>true if the pixel is the object, false if bg</returns>
    private bool PixelIsObject(float x, float y)
    {

        // The camera creating the scanningImage clears every pixel to a = 0, so if alpha > 0 something is there
        if (scanningImage.GetPixelBilinear(x, y).a > 0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Once the scene is done rendering, we copy the RenderTexture to a Texture2D we can scan through
    /// </summary>
    /// <param name="source">Unused</param>
    /// <param name="destination">Unused</param>
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Copy the pixels out of the renderTexture so we can look at them later
        scanningImage.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

        // Don't actually change the image, but unity will complain if I don't at least pretend
        Graphics.Blit(source, destination);
    }
    #endregion

    #region Extra Unneccessary things for center bracket which is also unnecccessary. 
    private void CenterBracketActive()
    {
        CenterBracket.enabled = true;          
    }
    private void CenterBracketInactive()
    {
        CenterBracket.enabled = false;
    }
    private void RotateCenterBracket()
    {      
        CenterBracket.transform.Rotate(0, centerBracketRotationSpeed * Time.deltaTime, 0, Space.World);
    }
    #endregion
}
