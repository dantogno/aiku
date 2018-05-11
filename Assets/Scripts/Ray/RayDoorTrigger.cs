using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayDoorTrigger : MonoBehaviour
{
    public static event Action PlayerEnteredTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerEnteredTrigger != null) PlayerEnteredTrigger.Invoke();
    }
}
