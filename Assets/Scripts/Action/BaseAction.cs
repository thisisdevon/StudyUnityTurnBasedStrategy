using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitScript))]
public abstract class BaseAction : MonoBehaviour
{
    [SerializeField] private int range = 0;
    public static event EventHandler OnAnyActionExecuted;
    public static event EventHandler OnAnyActionCompleted;
    protected UnitScript ownerUnit;
    protected bool isActive = false;
    protected Action onActionComplete;
    protected List<GridSystem.GridPosition> validGridPositionList;
    protected List<GridSystem.GridPosition> executableGridPositionList;
    protected abstract bool IsGridPositionExecutable(GridSystem.GridPosition gridPosition);
    protected abstract bool IsGridPositionValid(GridSystem.GridPosition gridPosition);
    
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
    
    public bool IsSelectedGridWithinExecutableList(GridSystem.GridPosition gridPosition)
    {
        return executableGridPositionList.Contains(gridPosition);
    }

    public virtual string GetActionName()
    {
        return "Action";
    }

    // Let the UnitActionSystemScript to handle the activation condition

    // to be handled by SelectSelectedAction 
    public virtual void ActionSelected(Action onActionComplete)
    {
        SetupActionGridPositionList();
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
        return IsSelectedGridWithinExecutableList(targetGridPosition) 
            && ownerUnit.TryToExecuteAction(this); 
    }

    // to be handled by CompletrSelectedAction 
    public virtual void ActionComplete()
    {
        isActive = false;
        onActionComplete();
        OnAnyActionCompleted?.Invoke(this, null);
    }


    protected void SetupActionGridPositionList()
    {
        executableGridPositionList = new List<GridSystem.GridPosition>();
        validGridPositionList = new List<GridSystem.GridPosition>();
        GridSystem.GridPosition currentGridPosition = ownerUnit.GetGridPosition();
        
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                int totalDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (totalDistance > range) 
                {
                    continue;
                }
                GridSystem.GridPosition offsetGridPosition = new GridSystem.GridPosition(x, z);
                GridSystem.GridPosition thisGridPosition = currentGridPosition + offsetGridPosition;

                if (IsGridPositionExecutable(thisGridPosition))
                {
                    executableGridPositionList.Add(thisGridPosition);
                }
                else if (IsGridPositionValid(thisGridPosition))
                {
                    validGridPositionList.Add(thisGridPosition);
                }
            }
        }
    }

    public List<GridSystem.GridPosition> GetExecutableActionGridPositionList()
    {
        return executableGridPositionList;
    }
    
    
    public List<GridSystem.GridPosition> GetValidActionGridPositionList()
    {
        return validGridPositionList;
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
