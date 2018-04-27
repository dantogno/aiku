using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Notifies all subscribers that the engine sequence at the beginning of the game has been completed and the fusion reactor is shut down
/// </summary>
public class EngineSequenceManager : MonoBehaviour {

    public static event System.Action OnShutdown;

    [SerializeField]
    [Tooltip("The Task that tells the player to shut everything down")]
    private Task shutdownTask;

	// Use this for initialization
	void Start () {
        shutdownTask.OnTaskCompleted += Shutdown;
	}

    private void Update()
    {
        // Developer shortcut, since this basically activates every feature in the game.
        if (Application.isEditor && Input.GetKeyDown(KeyCode.Period))
            Shutdown();
    }

    private void Shutdown()
    {
        if(OnShutdown != null)
        {
            OnShutdown.Invoke();
        }
    }
}
