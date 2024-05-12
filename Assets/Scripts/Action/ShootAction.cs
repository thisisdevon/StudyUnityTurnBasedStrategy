using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootAction : BaseAction
{
    [SerializeField] private int damage = 50;
    private enum State
    {
        Aiming,
        Shooting,
        Cooloff
    }
    public EventHandler<OnShootEventArgs> OnShooting;

    public class OnShootEventArgs : EventArgs
    {
        public UnitScript targetUnit;
        public UnitScript ownerUnit;
    }

    private State state;
    private int maxShootDistance = 7;
    private float stateTimer = 1.0f;
    private UnitScript targetUnit;
    private bool canShootBullet = true;
    private Vector3 targetUnitPosition;

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

        Quaternion targetRotation = Quaternion.LookRotation(targetUnitPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 30f);
    }
    
    public UnitScript GetTargetUnit()
    {
        return targetUnit;
    }

    public override string GetActionName()
    {
        return "SHOOT";
    }

    public override void ActionSelected(Action onActionComplete)
    {
        base.ActionSelected(onActionComplete);
    }

    public override bool ActionExecute(GridSystem.GridPosition targetGridPosition)
    {
        float aimingStateTimer = 1f;
        stateTimer = aimingStateTimer;
        targetUnit = LevelGridScript.Instance.GetUnitAtGridPosition(targetGridPosition);
        targetUnitPosition = targetUnit.transform.position;
        canShootBullet = true;
        state = State.Aiming;
        
        base.ActionExecute(targetGridPosition);
        if (isActive)
        {
            state = State.Aiming;
        }
        return isActive;
    }

    public override void ActionComplete()
    {
        base.ActionComplete(); //isactive false is here
    }

    protected override bool IsGridPositionValid(GridSystem.GridPosition gridPosition)
    {
        return
            gridPosition != ownerUnit.GetGridPosition() &&
            LevelGridScript.Instance.IsValidGridPosition(gridPosition);
    }

    protected override bool IsGridPositionExecutable(GridSystem.GridPosition gridPosition)
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
        OnShooting?.Invoke(this, new OnShootEventArgs 
        {
            targetUnit = targetUnit, 
            ownerUnit = ownerUnit
        });
        targetUnit.TakeDamage(damage);
    }
    
    public override GridSystemVisual.GridVisualType GetExecutableGridVisualType()
    {
        return GridSystemVisual.GridVisualType.Red;
    }

    public virtual GridSystemVisual.GridVisualType GetValidGridVisualType()
    {
        return GridSystemVisual.GridVisualType.RedSoft;
    }
}
