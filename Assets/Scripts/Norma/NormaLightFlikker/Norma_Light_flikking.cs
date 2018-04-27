using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Norma_Light_flikking : MonoBehaviour {

    [SerializeField] 
    private Light fire;

    [SerializeField]
    private float min_range;

    [SerializeField]
    private float max_range;

    [SerializeField]
    private float step_of_range;

    void FixedUpdate ()
    {
        //barrell light flickers like real fire you would not belive it !
        fire.intensity = Mathf.Lerp(fire.intensity, Random.Range(min_range, max_range),step_of_range); 
    }
}
