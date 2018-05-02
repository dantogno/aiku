using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class goes on the door that should activate when the Alignment Puzzle
/// has been completed.
/// If the puzzle is completed, the door opens.
/// If the puzzle is not completed, it stays closed.
/// </summary>
public class RingPuzzleDoor : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The origin point of this door. Used for psuedo-animation.")]
    private GameObject bridgeOriginPoint;
    [SerializeField]
    [Tooltip("The end point of this door. This is where it goes when it is open. Used for psuedo-animation")]
    private GameObject bridgeEndPoint;
    [SerializeField]
    [Tooltip("The puzzle which, upon completion, affects this object.")]
    private RingPuzzle puzzle;

    [SerializeField]
    [Tooltip("How long should it take the door to open/close?")]
    private float timeToAnimate = 2f;

    // A reference to the currently running open or close coroutine.
    private Coroutine currentCoroutine = null;

    /// <summary>
    /// Subscribe to when the puzzle is completed or uncompleted.
    /// </summary>
    private void OnEnable()
    {
        puzzle.PuzzleUnlocked += OnPuzzleUnlocked;
        puzzle.PuzzleLocked += OnPuzzleLocked; ;
    }
    /// <summary>
    /// Unsubscribe to when the puzzle is completed or uncompleted.
    /// </summary>
    private void OnDisable()
    {
        puzzle.PuzzleUnlocked -= OnPuzzleUnlocked;
        puzzle.PuzzleLocked -= OnPuzzleLocked; ;
    }

    /// <summary>
    /// Lerps the door to the end point over a period of time.
    /// </summary>
    private IEnumerator GrantAccess(float overTime)
    {
        float elapsedTime = 0;
        while(elapsedTime < overTime)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, bridgeEndPoint.transform.position, (elapsedTime / overTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Lerps the door to the origin point over a period of time.
    /// </summary>
    private IEnumerator DenyAccess(float overTime)
    {
        float elapsedTime = 0;
        while(elapsedTime < overTime)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, bridgeOriginPoint.transform.position, (elapsedTime / overTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// The door should be open.
    /// </summary>
    private void OnPuzzleUnlocked()
    {
        // Stops the closing coroutine if it has been started.
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(GrantAccess(timeToAnimate));
    }

    /// <summary>
    /// The door should be closed.
    /// </summary>
    private void OnPuzzleLocked()
    {
        // Stops the opening coroutine if is has been started.
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(DenyAccess(timeToAnimate));
    }
}
