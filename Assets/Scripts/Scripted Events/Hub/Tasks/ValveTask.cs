using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This task is for turning off all three of the valves in the engine room.
/// The script is applied to a task object under "Tasks" in the hierarchy.
/// </summary>

public class ValveTask : Task
{
    [SerializeField, Tooltip("The GameObject controlling the valve interaction sequence. Once all three valves are turned, the task is complete.")]
    private ValveInteractionSequence valveSequence;

    private void OnEnable()
    {
        valveSequence.AllThreeValvesTurned += SetComplete;
    }
    private void OnDisable()
    {
        valveSequence.AllThreeValvesTurned -= SetComplete;
    }

    protected override void SetComplete()
    {
        base.SetComplete();
    }
}
