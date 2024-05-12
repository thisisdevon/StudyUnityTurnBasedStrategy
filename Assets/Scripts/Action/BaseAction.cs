using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitScript))]
public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionExecuted;
    public static event EventHandler OnAnyActionCompleted;
    protected UnitScript ownerUnit;
    protected bool isActive = false;
    protected Action onActionComplete;
    protected List<GridSystem.GridPosition> validGridPositionList;
    
    protected virtual void Awake()
    {
        ownerUnit = GetComponent<UnitScript>();
        validGridPositionList = new List<GridSystem.GridPosition>();
    }
    public UnitScript GetOwnerUnit()
    {
        return ownerUnit;
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
        validGridPositionList = GetExecutableActionGridPositionList();
        this.onActionComplete = onActionComplete;
    }

    // to be handled by ExecuteSelectedAction 
    public virtual bool ActionExecute(GridSystem.GridPosition targetGridPosition)
    {
        isActive = CanExecute(targetGridPosition);
        if (isActive)
        {
            OnAnyActionExecuted?.Invoke(this, null);
        }
        return isActive;
    }

    protected bool CanExecute(GridSystem.GridPosition targetGridPosition)
    {
        return IsSelectedGridWithinValidList(targetGridPosition) 
            && ownerUnit.TryToExecuteAction(this); 
    }

    // to be handled by CompletrSelectedAction 
    public virtual void ActionComplete()
    {
        isActive = false;
        onActionComplete();
        OnAnyActionCompleted?.Invoke(this, null);
    }

    public virtual List<GridSystem.GridPosition> GetExecutableActionGridPositionList()
    {
        return new List<GridSystem.GridPosition>();
    }
    
    
    public virtual List<GridSystem.GridPosition> GetValidActionGridPositionList()
    {
        return new List<GridSystem.GridPosition>();
    }
    
    public virtual int GetActionPointsCost()
    {
        return 1;
    }
    
    public virtual GridSystemVisual.GridVisualType GetExecutableGridVisualType()
    {
        return GridSystemVisual.GridVisualType.White;
    }

    public virtual GridSystemVisual.GridVisualType GetValidGridVisualType()
    {
        return GridSystemVisual.GridVisualType.WhiteSoft;
    }
}
