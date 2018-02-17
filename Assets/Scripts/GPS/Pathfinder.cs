using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The "brain" of the GPSLine navigation, calculates paths between NavNodes
/// </summary>
public class Pathfinder : MonoBehaviour
{
    private List<NavNode> mapNodes = new List<NavNode>();
    public List<NavNode> MapNodes { get { return mapNodes; } private set { mapNodes = value; } }

    //Dictionary of distances, used to find a path back to the root from any NavNode
    private Dictionary<NavNode, float> distancesFromRoot = new Dictionary<NavNode, float>();

    //this is the node all the distances are calculated from, usually the target of navigation
    private NavNode currentRoot;

    private void OnDrawGizmosSelected()
    {
        foreach(NavNode node in MapNodes)
        {
            node.DrawConnections();
        }
    }

    private void Start()
    {
        //might be better to just use array the whole time...
        NavNode[] childNavNodes = GetComponentsInChildren<NavNode>();

        foreach(NavNode node in childNavNodes)
        {
            mapNodes.Add(node);
        }
    }

    /// <summary>
    /// calculates distance to root from every other node, must be called after changing the root
    /// </summary>
    /// <param name="root"></param>
    private void CalculateDistances(NavNode root)
    {
        //reset distances
        distancesFromRoot.Clear();
        foreach(NavNode node in MapNodes)
        {
            distancesFromRoot.Add(node, float.MaxValue);
        }
       
        //set up initial conditions for the search
        List<NavNode> visitedNodes = new List<NavNode>();
        visitedNodes.Add(root);
        Stack<Edge> edgesToVisit = new Stack<Edge>();
        distancesFromRoot[root] = 0;

        foreach(NavNode node in root.ConnectedNodes)
        {
            edgesToVisit.Push(new Edge(root, node));
        }


        //begin calculation of distances
        while(edgesToVisit.Count > 0)
        {
            //take an edge off the stack
            Edge currentEdge = edgesToVisit.Pop();

            //the current node is the one we just traveled to across the edge
            NavNode currentNode = currentEdge.to;

            //reduce the distance if the current route is shorter
            distancesFromRoot[currentNode] = Mathf.Min(
                distancesFromRoot[currentNode], //this is the length of the current shortest path back to root from the current node (or it's float.MaxValue)
                distancesFromRoot[currentEdge.from] + currentEdge.length); //but it might be faster to travel through the route we just took instead

            //if we haven't visited that node yet, add its connections as edges to the stack
            if (!visitedNodes.Contains(currentNode))
            {
                foreach(NavNode node in currentNode.ConnectedNodes)
                {
                    edgesToVisit.Push(new Edge(currentNode, node));
                }

                visitedNodes.Add(currentNode);
            }
        }
    }

    /// <summary>
    /// Returns a list of nodes forming a path to the target
    /// </summary>
    /// <param name="target">where we want to go</param>
    /// <param name="startPoint">the closest node to the player</param>
    /// <returns></returns>
    public List<NavNode> GetRouteTo(NavNode target, NavNode startPoint)
    {
        //if we want to find a route to a target that doesn't match the current distance dictionary, recalculate it
        if(currentRoot != target)
        {
            CalculateDistances(target);
            currentRoot = target;
        }

        List<NavNode> route = new List<NavNode>();

        NavNode currentNode = startPoint;

        //so that the game doesn't freeze if somebody asks for an unreachable target
        int infiniteLoop = 1000;
        int loopCount = 0;

        //follow the connections between nodes, always taking the shortest route to the target
        while(currentNode != target && loopCount < infiniteLoop)
        {
            NavNode nextNode = null;
            float shortestDistance = float.MaxValue;

            //of all connected nodes, find the one closest to root
            foreach(NavNode node in currentNode.ConnectedNodes)
            {
                float distanceToNode = Vector3.Magnitude(node.transform.position - currentNode.transform.position);
                if(distancesFromRoot[node] + distanceToNode < shortestDistance)
                {
                    nextNode = node;
                    shortestDistance = distancesFromRoot[node] + distanceToNode;
                }
            }

            route.Add(currentNode);
            currentNode = nextNode;

            loopCount++;
        }

        route.Add(target);

        return route;
    }
}


