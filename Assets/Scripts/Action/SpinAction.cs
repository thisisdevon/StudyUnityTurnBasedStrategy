using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    public class SpinActionParameters : BaseActionParameters
    {  
        public int spinCount = 0;

        public SpinActionParameters(int spinCount)
        {
            this.spinCount = spinCount;
        }
    }

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
        if (totalSpinAmount >= 360f * GetSpinCount())
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

    private int GetSpinCount()
    {
        return ((SpinActionParameters) baseActionParameter).spinCount;
    }

    public override string GetActionName()
    {
        return "SPIN";
    }

    public override void ActionSelected(Action onActionComplete)
    {
        base.ActionSelected(onActionComplete);
        ActionExecute(new SpinActionParameters(2)); // immediately activate
    }

    public override void ActionExecute(BaseActionParameters baseActionParameter)
    {
        base.ActionExecute(baseActionParameter);
    }

    public override void ActionComplete()
    {
        StopSpinning();
        base.ActionComplete();
    }

    protected override bool IsActionValidToBeExecuted()
    {
        return true;
    }
}
