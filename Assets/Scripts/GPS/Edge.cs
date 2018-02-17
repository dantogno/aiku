using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// utility class, connects two nodes and has a "cost" or length
/// </summary>
public class Edge
{
    public NavNode from, to;
    public float length;

    public Edge(NavNode _from, NavNode _to)
    {
        from = _from;
        to = _to;
        length = Vector3.Magnitude(to.transform.position - from.transform.position);
    }
}