using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableHandScene : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    [Tooltip("Drag the Hand Gameobject here. It should be the child of the main Camera")]
    private GameObject handGesture;

    private void OnTriggerEnter(Collider other)
    {
        handGesture.SetActive(true);
    }


}
