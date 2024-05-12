using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    private const int ACTION_POINTS_INIT = 2;
    public static event EventHandler OnAnyActionPointsChanged;
    [SerializeField] private bool isEnemy;

    private GridSystem.GridPosition currentGridPosition;
    private HealthSystem healthSystem;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;
    private int actionPoints;

    void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
        healthSystem = GetComponent<HealthSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        healthSystem.OnDeath += HealthSystem_OnDead;
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

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }

    private void ResetActionPoints()
    {
        SetActionPoints(ACTION_POINTS_INIT);
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
        UpdateGridPosition(LevelGridScript.Instance.GetGridPosition(transform.position));
    }

    public GridSystem.GridPosition GetGridPosition()
    {
        return currentGridPosition;
    }


    public Vector3 GetWorldPosition()
    {
        return LevelGridScript.Instance.GetWorldPosition(currentGridPosition);
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
        SetActionPoints(actionPoints - spentAmount);
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    private void SetActionPoints(int actionPoints)
    {
        this.actionPoints = actionPoints;
        OnAnyActionPointsChanged?.Invoke(this, null);
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void TakeDamage(int damageAmount)
    {
        if (!healthSystem.TakeDamage(damageAmount))
        {
            //already died
            LevelGridScript.Instance.RemoveUnitAtGridPosition(currentGridPosition, this);
        }
    }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }
}
