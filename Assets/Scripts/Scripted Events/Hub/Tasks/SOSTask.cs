using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOSTask : Task
{
    private void OnEnable()
    {
        SOSButton.PressedButton += SetComplete;
    }
    private void OnDisable()
    {
        SOSButton.PressedButton -= SetComplete;
    }

    protected override void SetComplete()
    {
        base.SetComplete();
    }
}
