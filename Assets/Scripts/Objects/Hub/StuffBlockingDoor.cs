using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffBlockingDoor : MonoBehaviour
{
    [SerializeField] private Rigidbody[] bullshit;

    private void OnEnable()
    {
        PickupGMD.PickedUpGMD += SetRigidbodiesNonKinematic;
    }
    private void OnDisable()
    {
        PickupGMD.PickedUpGMD -= SetRigidbodiesNonKinematic;
    }

    private void SetRigidbodiesNonKinematic()
    {
        foreach(Rigidbody r in bullshit)
        {
            r.isKinematic = false;
        }
    }
}
