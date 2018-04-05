using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLights : MonoBehaviour {
    public GameObject lightcone;
    public Light rotatelight;


    private void OnTriggerEnter(Collider other)
    {
        lightcone.SetActive(false);

    }
}
