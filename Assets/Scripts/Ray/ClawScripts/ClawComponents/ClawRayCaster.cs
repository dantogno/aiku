using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawRayCaster : MonoBehaviour {

    #region EditorFields
    //Where the raycast starts from the claw so that it 
    //isn't hitting the claw and the claw knows what it is over
    [Tooltip("The Transform of the raycastStartPos child of the Claw Game Object")]
    [SerializeField]
    private Transform raycastStartPosition;

    [Tooltip("The Line Renderer component of the Claw Game Object")]
    //The "Laser" that indicates if the claw is over anything or not.
    [SerializeField]
    private LineRenderer lineRenderer;

    [Tooltip("The distance the claw raycasts looking for an object")]
    [SerializeField]
    private int Raydistance; //How far the claw looks for an object
    #endregion

    #region PrivateFields
    private RaycastHit hit = new RaycastHit(); //What the claw is over

    private Vector3 raycastDirection; //The direction for the raycast (down) for the claw to check for objects
    #endregion

    public ClawLiftableCrate CurentCraneTarget { get; private set; }

    // Use this for initialization
    void Start()
    {
        raycastDirection = new Vector3(0, -1, 0);
    }

    private void Update()
    {
        ScanForViableTarget();
    }

    public void ScanForViableTarget()
    {
        // if our raycast hits something
        if (Physics.Raycast(raycastStartPosition.position, raycastDirection, out hit, Raydistance))
        {
            //Checks to see if the claw is over a clawLiftable object
            if (hit.transform.GetComponent<ClawLiftableCrate>() != null)
            {
                changeLaserColorToGreen();
                CurentCraneTarget = hit.transform.GetComponent<ClawLiftableCrate>();
            }

            else //The claw isnt pointing at a clawLiftableObject
            {
                changeLaserColorToRed();
                CurentCraneTarget = null;
            }
        }
    }

    public void TurnLaserOn()
    {
        lineRenderer.enabled = true; 
    }
    
    public void TurnLaserOff()
    {
        lineRenderer.enabled = false;
    }

    public RaycastHit getRaycastHit()
    {
      return hit;
    }

    private void changeLaserColorToGreen()
    {
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }

    private void changeLaserColorToRed()
    {
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

}
