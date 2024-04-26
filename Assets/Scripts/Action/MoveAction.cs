using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveAction : BaseAction
{
    public const float MOVE_SPEED = 4f;
    public const float ROTATE_SPEED = 10f;

    [SerializeField] private AnimationCurve moveCurve;
    [SerializeField] private int maxMoveDistance = 4;
    private Vector3 moveEndPosition;
    private Vector3 moveStartPosition;
    private float moveTime = -1f;
    private float elapsedMoveTime = 0f;

    protected override void Awake()
    {
        base.Awake();
        moveEndPosition = Vector3.zero;
        moveStartPosition = Vector3.zero;
        moveTime = -1f;
        elapsedMoveTime = 0f;
    }

    void Update()
    {
        if (!isActive) 
        {
            return;
        }
        if (IsMoving())
        {
            elapsedMoveTime += Time.deltaTime;
            float normalizedTime = elapsedMoveTime / moveTime;

            float curveEvaluate = moveCurve.Evaluate(normalizedTime);
            Vector3 newPosition = Vector3.Lerp(this.moveStartPosition, this.moveEndPosition, curveEvaluate);

            Quaternion targetRotation = Quaternion.LookRotation(this.moveEndPosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * ROTATE_SPEED);

            transform.position = newPosition;
        }
        else
        {
            ActionComplete();
        }
    }

    public bool IsMoving()
    {
        return elapsedMoveTime < moveTime && isActive;
    }

    private bool IsGridPositionValid(GridSystem.GridPosition gridPosition)
    {
        return
            gridPosition != ownerUnit.GetGridPosition() &&
            LevelGridScript.Instance.IsValidGridPosition(gridPosition) &&
            !LevelGridScript.Instance.IsUnitOnGridPosition(gridPosition)
        ;
    }

    public override string GetActionName()
    {
        return "MOVE";
    }

    public override void ActionSelected( Action onActionComplete)
    {
        base.ActionSelected(onActionComplete);
    }

    public override bool ActionExecute(GridSystem.GridPosition targetGridPosition)
    {
        this.moveEndPosition = LevelGridScript.Instance.GetWorldPosition(targetGridPosition);
        this.moveStartPosition = transform.position;

        elapsedMoveTime = 0f;

        moveTime = Vector3.Distance(this.moveEndPosition, this.moveStartPosition) / MOVE_SPEED;
        return base.ActionExecute(targetGridPosition);
    }

    public override void ActionComplete()
    {
        base.ActionComplete(); //isactive false is here
    }

    public override List<GridSystem.GridPosition> GetValidActionGridPositionList()
    {
        List<GridSystem.GridPosition> result = new List<GridSystem.GridPosition>();
        GridSystem.GridPosition currentGridPosition = ownerUnit.GetGridPosition();
        
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                int totalDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (totalDistance > maxMoveDistance) 
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
}
