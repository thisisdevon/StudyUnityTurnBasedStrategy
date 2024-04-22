using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    public event EventHandler OnUnitStopMoving;
    //public event EventHandler<UnitScript> OnUnitMoving;
    [SerializeField] private Animator unitAnimator;

    private GridSystem.GridPosition currentGridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;

    void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.currentGridPosition = LevelGridScript.Instance.GetGridPosition(transform.position);
        LevelGridScript.Instance.AssignUnit(this);
        LevelGridScript.Instance.GetGridObject(this.currentGridPosition).UnitEnterGrid(this);
    }

    public void UpdateGridPosition(GridSystem.GridPosition gridPosition)
    {
        if (this.currentGridPosition != gridPosition)
        {
            LevelGridScript.Instance.UnitChangedGridPosition(this, this.currentGridPosition, gridPosition);
            this.currentGridPosition = gridPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsMoving())
        {
            unitAnimator.SetBool("IsWalking", false);
            OnUnitStopMoving?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            unitAnimator.SetBool("IsWalking", true);
            //OnUnitMoving?.Invoke(this, this);
            UpdateGridPosition(LevelGridScript.Instance.GetGridPosition(transform.position));
        }
    }

    public GridSystem.GridPosition GetGridPosition()
    {
        return currentGridPosition;
    }

    //BASE ACTION

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    //MOVE ACTION

    public bool IsMoving () => moveAction.IsMoving(); 

    public bool Move(GridSystem.GridPosition targetGridPosition, Action actionEnd) => moveAction.Move(targetGridPosition, actionEnd);

    public List<GridSystem.GridPosition> GetValidActionGridPositionList() => moveAction.GetValidActionGridPositionList();

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    //

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }
}
