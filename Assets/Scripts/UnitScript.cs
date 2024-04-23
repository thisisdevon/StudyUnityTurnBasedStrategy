using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    private const int ACTION_POINTS_INIT = 2;
    public event EventHandler OnUnitStopMoving;
    public static event EventHandler OnAnyActionPointsChanged;
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private bool isEnemy;

    private GridSystem.GridPosition currentGridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;
    private int actionPoints;

    void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }

    // Start is called before the first frame update
    void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        this.currentGridPosition = LevelGridScript.Instance.GetGridPosition(transform.position);
        LevelGridScript.Instance.AssignUnit(this);
        LevelGridScript.Instance.GetGridObject(this.currentGridPosition).UnitEnterGrid(this);
        ResetActionPoints();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if ((IsEnemy() && !TurnSystem.Instance.IsEnemyTurn()) ||
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            ResetActionPoints();
        }
        
    }

    private void ResetActionPoints()
    {

        actionPoints = ACTION_POINTS_INIT;

        OnAnyActionPointsChanged?.Invoke(this, null);
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

    public List<GridSystem.GridPosition> GetValidActionGridPositionList() => moveAction.GetValidActionGridPositionList();

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public bool TryToExecuteAction(BaseAction baseAction)
    {
        if (CanExecuteAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        return false;
    }

    private bool CanExecuteAction(BaseAction baseAction)
    {
        return actionPoints >= baseAction.GetActionPointsCost();
    }

    private void SpendActionPoints(int spentAmount)
    {
        actionPoints -= spentAmount;
        OnAnyActionPointsChanged?.Invoke(this, null);
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }
}
