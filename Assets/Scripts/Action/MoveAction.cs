using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveAction : BaseAction
{
    public const float MOVE_SPEED = 4f;
    public const float ROTATE_SPEED = 10f;
    public EventHandler OnMoveActionExecute;
    public EventHandler OnMoveActionComplete;

    [SerializeField] private AnimationCurve moveCurve;
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

        base.ActionExecute(targetGridPosition);
        if (isActive)
        {
            OnMoveActionExecute?.Invoke(this, null);
        }
        return isActive;
    }

    public override void ActionComplete()
    {
        OnMoveActionComplete?.Invoke(this, null);
        base.ActionComplete(); //isactive false is here
    }

    protected override bool IsGridPositionExecutable(GridSystem.GridPosition gridPosition)
    {
        return
            gridPosition != ownerUnit.GetGridPosition() &&
            LevelGridScript.Instance.IsValidGridPosition(gridPosition) &&
            !LevelGridScript.Instance.IsUnitOnGridPosition(gridPosition);
    }
    
    protected override bool IsGridPositionValid(GridSystem.GridPosition gridPosition)
    {
        return
            gridPosition != ownerUnit.GetGridPosition() &&
            LevelGridScript.Instance.IsValidGridPosition(gridPosition);
        ;
    }

    
    public override GridSystemVisual.GridVisualType GetExecutableGridVisualType()
    {
        return GridSystemVisual.GridVisualType.White;
    }

    public override GridSystemVisual.GridVisualType GetValidGridVisualType()
    {
        return GridSystemVisual.GridVisualType.WhiteSoft;
    }
}
