using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatapadTextSingleton : MonoBehaviour {

    private DatapadTextSingleton self;

	// Use this for initialization
	void Start () {
        if (self == null)
        {
            self = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
