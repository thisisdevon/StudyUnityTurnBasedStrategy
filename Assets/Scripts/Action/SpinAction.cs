using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpinAmount = 0f;

    private void Spin()
    {
        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, 3f, 0);
        totalSpinAmount += spinAddAmount;
        if (totalSpinAmount >= 360f * 3)
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
    }

    public override bool ActionExecute(GridPosition targetGridPosition)
    {
        totalSpinAmount = 0f;
        return base.ActionExecute(targetGridPosition);
    }

    public override void ActionComplete()
    {
        base.ActionComplete();
    }

    public override int GetActionPointsCost()
    {
        return 1;
    }
    
    public override GridSystemVisual.GridVisualType GetExecutableGridVisualType()
    {
        return GridSystemVisual.GridVisualType.Blue;
    }

    protected override bool IsGridPositionExecutable(GridPosition gridPosition)
    {
        //hardcode to self
        return true;
    }

    protected override bool IsGridPositionValid(GridPosition gridPosition)
    {
        //hardcode to self
        return true;
    }

    protected override EnemyAIAction ValuateEnemyAIActionFromGridPosition(GridPosition gridPosition)
    {
        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }
}
