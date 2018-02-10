using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A task that is automatically completed after a given time has elapsed. Often used to wait for animations to play / give the player time to read
/// </summary>
public class TimedTask : Task {

    Coroutine SetCompleteCoroutine = null;

    [SerializeField]
    [Tooltip("The task will be complete this many seconds after it is set active. The timer can be reset if the task is set inactive before it runs out")]
    private float waitTime;

    public override void SetActive()
    {
        base.SetActive();

        SetCompleteCoroutine = StartCoroutine(SetCompleteAfterSeconds(waitTime));
    }

    public override void SetInactive()
    {
        base.SetInactive();

        if (SetCompleteCoroutine != null)
            StopCoroutine(SetCompleteCoroutine);
    }

    private IEnumerator SetCompleteAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        SetComplete();
    }
}
