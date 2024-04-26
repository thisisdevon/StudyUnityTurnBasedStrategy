using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    private enum State
    {
        Aiming,
        Shooting,
        Cooloff
    }

    private State state;
    private int maxShootDistance = 7;
    private float stateTimer = 1.0f;
    private UnitScript targetUnit;
    private bool canShootBullet = true;
    // Start is called before the first frame update
    void Start()
    {
        state = State.Aiming; 
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            return;
        }
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
        {
            NextState();
        }

        if (state == State.Shooting && canShootBullet)
        {
            canShootBullet = false;
            StartShoot();
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetUnit.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 60f);
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
        bool result = IsTheUnitOnGridShootable(targetGridPosition);
        if (result)
        {
            float aimingStateTimer = 1f;
            stateTimer = aimingStateTimer;
        }

        targetUnit = LevelGridScript.Instance.GetUnitAtGridPosition(targetGridPosition);
        canShootBullet = true;
        isActive = result &= base.CanExecute(targetGridPosition);
        return result;
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

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTimer = 0.1f;
                stateTimer = shootingStateTimer;
                break;

            case State.Shooting:
                state = State.Cooloff;
                float coolOffStateTimer = 0.5f;
                stateTimer = coolOffStateTimer;
                break;

            case State.Cooloff:
                ActionComplete();
                break;
        }
    }

    private void StartShoot()
    {
        targetUnit.TakeDamage();
    }
}