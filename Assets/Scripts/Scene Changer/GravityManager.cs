using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    private Vector3 originalGravity;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        originalGravity = Physics.gravity;
        SceneChanger.SceneChangeStarted += DisableGravity;
        SceneChanger.SceneChangeFinished += EnableGravity;
    }

    private void DisableGravity()
    {
        Physics.gravity = Vector3.zero;
    }

    private void EnableGravity()
    {
        Physics.gravity = originalGravity;
    }
}
