using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// used by the Pathfinder and GPSDisplay to find routes around the ship
/// </summary>
public class NavNode : MonoBehaviour
{

    public List<NavNode> ConnectedNodes { get { return connectedNodes; } private set { connectedNodes = value; } }

    [SerializeField]
    [Tooltip("nearby nodes you can walk to")]
    private List<NavNode> connectedNodes;

    private void OnDrawGizmosSelected()
    {
        DrawConnections();
    }

    /// <summary>
    /// used in the editor to see which nodes are connected
    /// </summary>
    public void DrawConnections()
    {
        Gizmos.color = Color.white;
        foreach(NavNode node in ConnectedNodes)
        {
            Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }
}
