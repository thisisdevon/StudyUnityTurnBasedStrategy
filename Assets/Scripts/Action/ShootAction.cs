using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{

    private int maxShootDistance = 7;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override string GetActionName()
    {
        return "SHOOT";
    }

    public override void ActionSelected( Action onActionComplete)
    {
        base.ActionSelected(onActionComplete);
    }

    public override bool ActionExecute(GridSystem.GridPosition targetGridPosition)
    {
        return IsTheUnitOnGridShootable(targetGridPosition) && base.ActionExecute(targetGridPosition);
    }

    public override void ActionComplete()
    {
        base.ActionComplete(); //isactive false is here
    }

    public override List<GridSystem.GridPosition> GetValidActionGridPositionList()
    {
        List<GridSystem.GridPosition> result = new List<GridSystem.GridPosition>();
        GridSystem.GridPosition currentGridPosition = ownerUnit.GetGridPosition();
        
        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                int totalDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (totalDistance > maxShootDistance) 
                {
                    continue;
                }
                GridSystem.GridPosition offsetGridPosition = new GridSystem.GridPosition(x, z);
                GridSystem.GridPosition thisGridPosition = currentGridPosition + offsetGridPosition;

                if (IsGridPositionValid(thisGridPosition))
                {
                    result.Add(thisGridPosition);
                }
            }
        }
        return result;
    }

    

    private bool IsGridPositionValid(GridSystem.GridPosition gridPosition)
    {
        return
            gridPosition != ownerUnit.GetGridPosition() &&
            LevelGridScript.Instance.IsValidGridPosition(gridPosition);
    }

    private bool IsTheUnitOnGridShootable(GridSystem.GridPosition gridPosition)
    {
        return
            LevelGridScript.Instance.IsUnitOnGridPosition(gridPosition) &&
            ownerUnit.IsEnemy() != LevelGridScript.Instance.GetUnitAtGridPosition(gridPosition).IsEnemy();
    }
}
