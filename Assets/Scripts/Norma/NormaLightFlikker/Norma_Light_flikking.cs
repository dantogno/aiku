using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Norma_Light_flikking : MonoBehaviour {

    [SerializeField] 
    private Light fire;

        

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        fire.intensity = Mathf.Lerp(fire.intensity, Random.Range(0.5f, 1.3f),0.25f);


    }
}
