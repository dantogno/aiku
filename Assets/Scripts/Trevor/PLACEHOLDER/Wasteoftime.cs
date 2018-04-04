using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wasteoftime : MonoBehaviour {

    [SerializeField] private GameObject portalphone; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            portalphone.SetActive(true);
        }
    }
}
