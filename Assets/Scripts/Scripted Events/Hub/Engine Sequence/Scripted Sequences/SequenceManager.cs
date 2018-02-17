using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gives player tasks in order and monitors their completion.
/// Put tasks in order as empty child objects.
/// </summary>
public class SequenceManager : MonoBehaviour {

    [SerializeField]
    [Tooltip("GPSDisplay attached to the player, used to navigate to objectives")]
    private GPSDisplay gps;

    //list of tasks to be completed in order
    private List<Task> Tasks = new List<Task>();

    //task the player must complete next
    private Task currentTask;
    public Task CurrentTask
    {
        get
        {
            return currentTask;
        }

        private set
        {
            currentTask = value;
        }
    }

    //position in the task list
    private int currentTaskIndex = 0;

    // Use this for initialization
    void Start () {

        //get all the child tasks in order
		for(int i = 0; i < transform.childCount; i++)
        {
            Task t = transform.GetChild(i).GetComponent<Task>();

            if(t != null)
            {
                Tasks.Add(t);
            }
        }

        StartTasks();
	}

    /// <summary>
    /// sets the first task as the active one
    /// </summary>
    private void StartTasks()
    {
        currentTaskIndex = -1;
        NextTask();
    }

    /// <summary>
    /// called whenever the current task's "OnTaskCompleted" event fires
    /// </summary>
    public void TaskCompletedHandler()
    {
        NextTask();
    }

    /// <summary>
    /// unsubscribes from the current task and subscribes to the next one
    /// </summary>
    private void NextTask()
    {
        //unsubscribe from the current task
        if(CurrentTask != null)
            CurrentTask.OnTaskCompleted -= TaskCompletedHandler;

        currentTaskIndex++;

        //if there are more tasks, subscribe to the next one
        if(Tasks.Count > currentTaskIndex)
        {
            CurrentTask = Tasks[currentTaskIndex];

            CurrentTask.OnTaskCompleted += TaskCompletedHandler;
            CurrentTask.SetActive();

            gps.SetNavigationTarget(CurrentTask.ObjectiveNode);
        }
        //otherwise, we are done with the sequence
        else
        {
            Debug.Log("completed all tasks");
        }
    }
}
