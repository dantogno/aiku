using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Something the player has to do to advance the game
/// </summary>
public class Task : MonoBehaviour{

    protected bool complete = false;
    protected bool active = false;

    [SerializeField]
    [Tooltip("Description of the task")]
    protected string objectiveText;
    public string ObjectiveText { get { return objectiveText; } protected set { objectiveText = value; } }

    [SerializeField]
    [Tooltip("NavNode nearest to where the task can be completed")]
    protected NavNode objectiveNode;
    public NavNode ObjectiveNode { get { return objectiveNode; } protected set { objectiveNode = value; } }

    /// <summary>
    /// true if the player has completed the task
    /// </summary>
    public bool Complete { get { return complete; } protected set { complete = value; } }
    /// <summary>
    /// true if this task can be completed next
    /// </summary>
    public bool Active { get { return active; } protected set { active = value; } }

    //unused here, but made protected so that it can be overriden
    protected virtual void Start()
    {

    }

    /// <summary>
    /// sets the task to be active
    /// </summary>
    public virtual void SetActive()
    {
        Active = true;
    }

    /// <summary>
    /// sets the task to be inactive
    /// </summary>
    public virtual void SetInactive()
    {
        Active = false;
    }

    /// <summary>
    /// called when player completes the task
    /// </summary>
    protected virtual void SetComplete()
    {
        Active = false;
        Complete = true;
        
        if(OnTaskCompleted != null)
        {
            OnTaskCompleted.Invoke();
        }  
    }

    /// <summary>
    /// subscribe to this event to be notified when the task is completed
    /// </summary>
    public event System.Action OnTaskCompleted;
}
