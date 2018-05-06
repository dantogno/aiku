using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCanvas : MonoBehaviour
{
    private TransitionCanvas instance;

    private void Awake()
    {
        this.gameObject.SetActive(false);
        bool exists = CheckForSingleton();
        if(exists)
        {
            return;
        }
        SceneTransition.SceneChangeStarted += EnableThis;
        SceneTransition.SceneChangeFinished += DisableThis;
    }

    private bool CheckForSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            return false;
        }
        else
        {
            Destroy(this.gameObject);
            return true;
        }
    }

    //private void Update()
    //{
    //    //Logic for setting alpha value of text.
    //}

    private void EnableThis()
    {
        this.gameObject.SetActive(true);
    }

    private void DisableThis()
    {
        this.gameObject.SetActive(false);
    }
}
