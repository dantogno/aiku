using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject boulderWall;
    [SerializeField] private GameObject FinalTrigger;
    Rigidbody[] CeilingTiles;

    private void Start()
    {
        CeilingTiles = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody cTile in CeilingTiles)
        {
            cTile.isKinematic = true;
            cTile.useGravity = false;
        }
        gameObject.GetComponent<BoxCollider>().enabled = false;
        FinalTrigger.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            boulderWall.SetActive(false);
            FinalTrigger.SetActive(true);
            foreach (Rigidbody cTile in CeilingTiles)
            {
                cTile.isKinematic = false;
                cTile.useGravity = true;
            }

        }
    }

}
