using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSDisplay : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Pathfinder with the map in it")]
    private Pathfinder pathFinder;

    [SerializeField]
    [Tooltip("LineRenderer with a GPSLine shader on it")]
    private LineRenderer GPSLine;

    [SerializeField]
    [Tooltip("how often we recalculate the path to the current navigation target")]
    private float timeBetweenUpdates = 0.2f;

    private NavNode closestNode;
    private NavNode targetNode;

    //sequence of nodes between closestNode and targetNode
    private List<NavNode> currentRoute = new List<NavNode>();

    private float timeOfLastUpdate = 0;

	// Use this for initialization
	private void Awake ()
    {
        targetNode = null;
	}
	
	// Update is called once per frame
	private void Update ()
    {

        //if time to update
		if(Time.time > timeOfLastUpdate + timeBetweenUpdates)
        {
            //hide the line if there is no target
            if(targetNode == null)
            {
                GPSLine.enabled = false;
            }
            //if we have a target, draw a line to it
            if (targetNode != null)
            {
                GPSLine.enabled = true;
                CalculateNearestNavNode();
                currentRoute = pathFinder.GetRouteTo(targetNode, closestNode);
                DrawGPSLine();
            }

            timeOfLastUpdate = Time.time;
        }
	}

    /// <summary>
    /// sets closestNode to be the closest node to this gameobject's transform
    /// </summary>
    private void CalculateNearestNavNode()
    {
        float shortestDistance = float.MaxValue;
        foreach(NavNode node in pathFinder.MapNodes)
        {
            float distance = Vector3.SqrMagnitude(node.transform.position - transform.position);

            if(distance < shortestDistance)
            {
                closestNode = node;
                shortestDistance = distance;
            }
        } 
    }

    /// <summary>
    /// moves the GPSLine LineRenderer so that it traces the current route
    /// </summary>
    private void DrawGPSLine()
    {
        Vector3[] linePoints = new Vector3[currentRoute.Count];

        for (int i = 0; i < currentRoute.Count; i++)
        {
            linePoints[currentRoute.Count - 1 - i] = currentRoute[i].transform.position;
        }

        GPSLine.positionCount = linePoints.Length;
        GPSLine.SetPositions(linePoints);
    }

    /// <summary>
    /// sets targetNode
    /// </summary>
    /// <param name="target">the node we wish to navigate to</param>
    public void SetNavigationTarget(NavNode target)
    {
        targetNode = target;
    }
}
