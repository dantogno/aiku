using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// adds a button to the pathfinder inspector
/// </summary>
[CustomEditor(typeof(Pathfinder))]
public class PathfinderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Make nav node connections two-way"))
        {
            MakeConnectionsTwoWay();
        }
    }

    private void MakeConnectionsTwoWay()
    {
        Pathfinder pathfinder = target as Pathfinder;
        NavNode[] nodes = pathfinder.GetComponentsInChildren<NavNode>();

        foreach (NavNode node in nodes)
        {
            foreach (NavNode neighboringNode in node.ConnectedNodes)
            {
                //if the other node doesn't have a connection back to this node, create it
                //this just saves designers time
                if (!neighboringNode.ConnectedNodes.Contains(node))
                {
                    neighboringNode.ConnectedNodes.Add(node);
                }
            }
        }
    }
}
