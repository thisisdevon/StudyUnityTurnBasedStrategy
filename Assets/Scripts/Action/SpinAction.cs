using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpinAmount = 0f;

    public void StartSpinning()
    {
        //no need to do anything
    }

    public void StopSpinning()
    {
        totalSpinAmount = 0f;
    }

    private void Spin()
    {
        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, 3f, 0);
        totalSpinAmount += spinAddAmount;
        if (totalSpinAmount >= 720f)
        {
            ActionComplete();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            return;
        }
        Spin();
    }

    public override string GetActionName()
    {
        return "SPIN";
    }

    public override void ActionSelected(Action onActionComplete)
    {
        base.ActionSelected(onActionComplete);
        ActionExecute(); // immediately activate
    }

    public override void ActionExecute()
    {
        StartSpinning();
        base.ActionExecute();
    }

    public override void ActionComplete()
    {
        StopSpinning();
        base.ActionComplete();
    }
}
