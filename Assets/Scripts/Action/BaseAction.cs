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
    protected List<GridPosition> validGridPositionList;
    protected List<GridPosition> executableGridPositionList;
    protected abstract bool IsGridPositionExecutable(GridPosition gridPosition);
    protected abstract bool IsGridPositionValid(GridPosition gridPosition);
    
    protected virtual void Awake()
    {
        ownerUnit = GetComponent<UnitScript>();
        validGridPositionList = new List<GridPosition>();
    }
    public UnitScript GetOwnerUnit()
    {
        return ownerUnit;
    }

    public bool GetIsActive()
    {
        return isActive;
    }

    public bool IsSelectedGridWithinValidList(GridPosition gridPosition)
    {
        return validGridPositionList.Contains(gridPosition);
    }
    
    public bool IsSelectedGridWithinExecutableList(GridPosition gridPosition)
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
    public virtual bool ActionExecute(GridPosition targetGridPosition)
    {
        isActive = TryToExecuteAction(targetGridPosition);
        if (isActive)
        {
            OnAnyActionExecuted?.Invoke(this, null);
        }
        return isActive;
    }

    protected bool TryToExecuteAction(GridPosition targetGridPosition)
    {
        return IsSelectedGridWithinExecutableList(targetGridPosition) 
            && ownerUnit.TryToExecuteAction(this); 
    }

    public bool CanExecuteAction()
    {
        return ownerUnit.CanExecuteAction(this);
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
        executableGridPositionList = new List<GridPosition>();
        validGridPositionList = new List<GridPosition>();
        GridPosition currentGridPosition = ownerUnit.GetGridPosition();
        
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                int totalDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (totalDistance > range) 
                {
                    continue;
                }
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition thisGridPosition = currentGridPosition + offsetGridPosition;

                if (IsGridPositionExecutable(thisGridPosition))
                {
                    //if the action can be executed to this grid
                    executableGridPositionList.Add(thisGridPosition);
                }
                else if (IsGridPositionValid(thisGridPosition))
                {
                    //if the grid is within range but the action cannot be executed
                    validGridPositionList.Add(thisGridPosition);
                }
            }
        }
    }

    protected List<GridPosition> GetExecutableGridPositionsFromAGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> result = new List<GridPosition>();
        
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                int totalDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (totalDistance > range) 
                {
                    continue;
                }
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition thisGridPosition = gridPosition + offsetGridPosition;

                if (IsGridPositionExecutable(thisGridPosition))
                {
                    //if the action can be executed to this grid
                    result.Add(thisGridPosition);
                }
            }
        }
        return result;
    }

    public List<GridPosition> GetExecutableActionGridPositionList()
    {
        return executableGridPositionList;
    }
    
    
    public List<GridPosition> GetValidActionGridPositionList()
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

    public EnemyAIAction GetBestEnemyAIAction()
    {
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();

        List<GridPosition> executableGridPositionFromGridPositionList = GetExecutableGridPositionsFromAGridPosition(ownerUnit.GetGridPosition());

        foreach (GridPosition gridPosition in executableGridPositionFromGridPositionList)
        {
            EnemyAIAction enemyAIAction = ValuateEnemyAIActionFromGridPosition(gridPosition);
            enemyAIActionList.Add(enemyAIAction);
        }
        if (enemyAIActionList.Count == 0)
        {
            return null;
        }

        enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
        return enemyAIActionList[0];
    }

    protected abstract EnemyAIAction ValuateEnemyAIActionFromGridPosition(GridPosition gridPosition);
}
