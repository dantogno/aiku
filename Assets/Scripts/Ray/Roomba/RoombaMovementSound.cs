using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaMovementSound : MonoBehaviour
{
    [SerializeField, Tooltip("The Rigidbody which is moving, whose speed we can track.")]
    private Rigidbody roombaRigidbody;

    [SerializeField, Tooltip("The AudioSource playing the roomba noise.")]
    private AudioSource roombaSource;

    private bool canPlayAudio = false;

    private void Update()
    {
        canPlayAudio = (roombaRigidbody.velocity != Vector3.zero && !roombaSource.isPlaying);

        if (canPlayAudio)
        {
            roombaSource.Play();
        }
        else if (roombaRigidbody.velocity == Vector3.zero)
        {
            roombaSource.Stop();
        }
    }
}
