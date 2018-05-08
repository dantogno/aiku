using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A UI element that can be targeted by the brackets on the title screen
/// </summary>
public class TitleScreenBracketTarget : MonoBehaviour {

    /// <summary>
    /// The fake version of the brackets used on the title screen
    /// </summary>
    private TitleScreenBrackets brackets;

    /// <summary>
    /// This object's rect transform, passed to the brackets to select it
    /// </summary>
    private RectTransform rectTransform;

	// Use this for initialization
	void Start () {
        rectTransform = GetComponent<RectTransform>();
        brackets = FindObjectOfType<TitleScreenBrackets>();
	}

    /// <summary>
    /// Tell the brackets to select this object
    /// </summary>
    public void SelectThis()
    {
        brackets.Select(rectTransform);
    }

    /// <summary>
    /// Return the brackets to the default position
    /// </summary>
    public void SelectNothing()
    {
        brackets.Select(null);
    }
}
