using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Task that requires the player to interact with a console in the engine area
/// </summary>
public class EngineScreenTask : Task {

    [SerializeField]
    private InteractableEngineScreen EngineScreen;

	// Use this for initialization
	protected override void Start () {
        EngineScreen.OnInteracted += SetComplete;	
	}

    public override void SetActive()
    {
        base.SetActive();
        EngineScreen.SetActive();
    }

    protected override void SetComplete()
    {
        base.SetComplete();
    }

    public override void SetInactive()
    {
        base.SetInactive();
        EngineScreen.SetInactive();
    }
}
