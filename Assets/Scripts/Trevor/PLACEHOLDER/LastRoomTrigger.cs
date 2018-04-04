using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastRoomTrigger : MonoBehaviour
{
    [SerializeField] private GameObject finalPortal;
    [SerializeField] private GlitchyEffect glitchEffect;
    [SerializeField] private GameObject arrow;

    private void OnEnable()
    {
        arrow.SetActive(true);
    }
    private void OnDisable()
    {
        arrow.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            finalPortal.SetActive(true);
            glitchEffect.Threshold = 0.2f;
            gameObject.SetActive(false);
        }
    }

}
