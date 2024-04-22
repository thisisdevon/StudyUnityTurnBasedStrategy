using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpinAmount = 0f;

    public void StartSpinning(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;
    }

    public void StopSpinning()
    {
        isActive = false;
        totalSpinAmount = 0f;
    }

    private void Spin()
    {
        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, 3f, 0);
        totalSpinAmount += spinAddAmount;
        if (totalSpinAmount >= 720f)
        {
            StopSpinning();
            onActionComplete();
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
}
