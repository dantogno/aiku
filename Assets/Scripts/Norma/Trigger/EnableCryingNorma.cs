using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCryingNorma : MonoBehaviour {

    [SerializeField]
    [Tooltip("Drag the animator for crying Norma Here")]
    private Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        anim.Play("NormaRunningAwayFromHouse");
    }

}
