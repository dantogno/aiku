using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSLineTester : MonoBehaviour
{

    [SerializeField]
    GPSDisplay gps;

    [SerializeField]
    NavNode targetNode;

	// Use this for initialization
	void Start () {
        gps.SetNavigationTarget(targetNode);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
