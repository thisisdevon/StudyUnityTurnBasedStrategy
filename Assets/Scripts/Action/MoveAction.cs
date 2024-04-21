using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitScript))]
public class MoveAction : MonoBehaviour
{
    public const float MOVE_SPEED = 4f;
    public const float ROTATE_SPEED = 10f;

    [SerializeField] private AnimationCurve moveCurve;
    [SerializeField] private int maxMoveDistance = 4;
    private UnitScript ownerUnit;
    private Vector3 moveEndPosition;
    private Vector3 moveStartPosition;
    private float moveTime = -1f;
    private float elapsedMoveTime = 0f;

    void Awake()
    {
        ownerUnit = GetComponent<UnitScript>();
    }

    void Start()
    {
        moveTime = -1f;
        elapsedMoveTime = 0f;
    }

    void Update()
    {
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
    }

    public bool Move(GridSystem.GridPosition targetGridPosition)
    {
        List<GridSystem.GridPosition> validGridPositionList = GetValidActionGridPositionList(); 
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
        return elapsedMoveTime < moveTime;
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
}
