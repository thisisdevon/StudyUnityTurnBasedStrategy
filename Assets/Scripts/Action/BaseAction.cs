using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitScript))]
public abstract class BaseAction : MonoBehaviour
{
    protected UnitScript ownerUnit;
    protected bool isActive = false;
    protected Action onActionComplete;
    protected List<GridSystem.GridPosition> validGridPositionList;


    protected virtual void Awake()
    {
        ownerUnit = GetComponent<UnitScript>();
        validGridPositionList = new List<GridSystem.GridPosition>();
    }

    public bool GetIsActive()
    {
        return isActive;
    }

    public bool IsSelectedGridWithinValidList(GridSystem.GridPosition gridPosition)
    {
        return validGridPositionList.Contains(gridPosition);
    }


    public virtual string GetActionName()
    {
        return "Action";
    }

    // Let the UnitActionSystemScript to handle the activation condition

    // to be handled by SelectSelectedAction 
    public virtual void ActionSelected(Action onActionComplete)
    {
        validGridPositionList = GetValidActionGridPositionList();
        GridSystemVisual.Instance.UpdateGridVisual();
        this.onActionComplete = onActionComplete;
    }

    // to be handled by ExecuteSelectedAction 
    public virtual bool ActionExecute(GridSystem.GridPosition targetGridPosition)
    {
        isActive = IsSelectedGridWithinValidList(targetGridPosition) && ownerUnit.TryToExecuteAction(this);
        return isActive;
    }

    // to be handled by CompletrSelectedAction 
    public virtual void ActionComplete()
    {
        isActive = false;
        onActionComplete();
        GridSystemVisual.Instance.UpdateGridVisual();
    }

    public virtual List<GridSystem.GridPosition> GetValidActionGridPositionList()
    {
        return new List<GridSystem.GridPosition>();
    }

    public virtual int GetActionPointsCost()
    {
        return 1;
    }
}
