using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveAction : BaseAction
{
    public class MoveActionParameters : BaseActionParameters
    {  
        public GridSystem.GridPosition targetGridPosition;

        public MoveActionParameters(GridSystem.GridPosition targetGridPosition)
        {
            this.targetGridPosition = targetGridPosition;
        }
    }

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
            onActionComplete();
            isActive = false;
        }
    }

    public bool TryToSetMoveParameters()
    {
        List<GridSystem.GridPosition> validGridPositionList = GetValidActionGridPositionList(); 
        GridSystem.GridPosition targetGridPosition = ((MoveActionParameters) baseActionParameter).targetGridPosition;
        if (!validGridPositionList.Contains(targetGridPosition))
        {
            return false;
        }
        this.moveEndPosition = LevelGridScript.Instance.GetWorldPosition(targetGridPosition);
        this.moveStartPosition = transform.position;

        elapsedMoveTime = 0f;

        moveTime = Vector3.Distance(this.moveEndPosition, this.moveStartPosition) / MOVE_SPEED;
        return true;
    }

    public bool IsMoving()
    {
        return elapsedMoveTime < moveTime && isActive;
    }

    public List<GridSystem.GridPosition> GetValidActionGridPositionList()
    {
        GridSystem.GridPosition currentGridPosition = ownerUnit.GetGridPosition();
        List<GridSystem.GridPosition> validGridPositionList = new List<GridSystem.GridPosition>();
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridSystem.GridPosition offsetGridPosition = new GridSystem.GridPosition(x, z);
                GridSystem.GridPosition thisGridPosition = currentGridPosition + offsetGridPosition;

                if (IsGridPositionValid(thisGridPosition))
                {
                    validGridPositionList.Add(thisGridPosition);
                }
            }
        }
        return validGridPositionList;
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
        GridSystemVisual.Instance.UpdateGridVisual();
        base.ActionSelected(onActionComplete);
    }

    public override void ActionExecute(BaseActionParameters baseActionParameter)
    {
        base.ActionExecute(baseActionParameter); //isactive true is here
    }

    public override void ActionComplete()
    {
        GridSystemVisual.Instance.UpdateGridVisual();
        base.ActionComplete(); //isactive false is here
    }

    protected override bool IsActionValidToBeExecuted()
    {
        return TryToSetMoveParameters();
    }
}
