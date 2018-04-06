using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeInstance : MonoBehaviour
{
    public static void TurnOnShader(GameObject manipulate)
    {
        EdgeAttach retrievedEdge = manipulate.GetComponent<EdgeAttach>();
        if (retrievedEdge == null)
        {
            manipulate.AddComponent<EdgeAttach>();
        }
        else
            retrievedEdge.enabled = true;
    }

    public static void DisableShader(GameObject manipulate)
    {
        EdgeAttach outline = manipulate.GetComponent<EdgeAttach>();
        if (outline != null && outline.enabled)
        {
            outline.enabled = false;
        }
    }

    public static void DestroyShader(GameObject manipulate)
    {
        EdgeAttach toDestroy = manipulate.GetComponent<EdgeAttach>();
        if (toDestroy != null)
        {
            Destroy(toDestroy);
        }
    }
}