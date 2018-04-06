using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This task is for turning the big valve in the engine room.
/// The script is applied to a task object under "Tasks" in the hierarchy.
/// </summary>

public class ButterflyValveTask : Task
{
    [SerializeField, Tooltip("The GameObject controlling the valve interaction sequence. Once the big valve is turned, the task is complete.")]
    private ValveInteractionSequence valveSequence;

    private void OnEnable()
    {
        valveSequence.GeneratorBlewUp += SetComplete;
    }
    private void OnDisable()
    {
        valveSequence.GeneratorBlewUp -= SetComplete;
    }

    protected override void SetComplete()
    {
        base.SetComplete();
    }
}
