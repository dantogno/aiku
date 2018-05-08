using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simulates the brackets on the title screen UI
/// </summary>
public class TitleScreenBrackets : MonoBehaviour {

    [SerializeField]
    [Tooltip("RectTransform of the panel the brackets are anchored in")]
    private RectTransform bracketArea;

    [SerializeField]
    [Tooltip("Rect the brackets return to when nothing is selected")]
    private Rect defaultBracketArea;

    [SerializeField]
    [Tooltip("Bracket movement settings")]
    private float bracketMoveTime = 0.1f, bracketMaxSpeed = 1000f;

    /// <summary>
    /// Used by SmoothDamp to track how quickly the brackets' anchors are moving each frame
    /// </summary>
    private Vector2 bracketAnchorMinVelocity, bracketAnchorMaxVelocity;

    /// <summary>
    /// The target rect we SmoothDamp towards
    /// </summary>
    private Rect targetArea;

    /// <summary>
    /// Move the brackets so they surround this UI element
    /// </summary>
    /// <param name="selection">The element to surround</param>
    public void Select(RectTransform selection)
    {
        if(selection != null)
        {
            float x, y, width, height;

            //convert from screen coords to normalized positions
            x = selection.anchoredPosition.x / Screen.width;
            y = selection.anchoredPosition.y / Screen.height;

            width = selection.rect.width / Screen.width;
            height = selection.rect.height / Screen.height;

            //set the target
            targetArea = new Rect(x, y, width, height);
        }
        else //return to normal
        {
            targetArea = defaultBracketArea;
        }
    }

    // Use this for initialization
    void Start () {
        targetArea = defaultBracketArea;
	}
	
	// Update is called once per frame
	void Update () {
        DrawBrackets(targetArea);
	}

    /// <summary>
    /// Performs SmoothDamp to smoothly move the brackets
    /// </summary>
    /// <param name="rect">target to surround</param>
    private void DrawBrackets(Rect rect)
    {
        bracketArea.anchorMin = Vector2.SmoothDamp(bracketArea.anchorMin, new Vector2(rect.min.x, rect.min.y), ref bracketAnchorMinVelocity, bracketMoveTime, bracketMaxSpeed, Time.deltaTime);

        bracketArea.anchorMax = Vector2.SmoothDamp(bracketArea.anchorMax, new Vector2(rect.max.x, rect.max.y), ref bracketAnchorMaxVelocity, bracketMoveTime, bracketMaxSpeed, Time.deltaTime);
    }
}
