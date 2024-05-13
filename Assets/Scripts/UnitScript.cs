using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    private const int ACTION_POINTS_INIT = 2;
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;
    [SerializeField] private bool isEnemy;

    private GridSystem.GridPosition currentGridPosition;
    private HealthSystem healthSystem;
    private BaseAction[] baseActionArray;
    private int actionPoints;

    void Awake()
    {
        baseActionArray = GetComponents<BaseAction>();
        healthSystem = GetComponent<HealthSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        OnAnyUnitSpawned?.Invoke(this, null);
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        healthSystem.OnDeath += HealthSystem_OnDead;
        this.currentGridPosition = LevelGridScript.Instance.GetGridPosition(transform.position);
        LevelGridScript.Instance.GetGridObject(this.currentGridPosition).UnitEnterGrid(this);
        ResetActionPoints();
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

    public bool IsMoving () => GetAction<MoveAction>().IsMoving();

    public bool TryToExecuteAction(BaseAction baseAction)
    {
        if (CanExecuteAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        return false;
    }

    public bool CanExecuteAction(BaseAction baseAction)
    {
        return actionPoints >= baseAction.GetActionPointsCost();
    }

    private void SpendActionPoints(int spentAmount)
    {
        Debug.Log(spentAmount);
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
        }
    }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
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
        LevelGridScript.Instance.RemoveUnitAtGridPosition(currentGridPosition, this);
        Destroy(gameObject);
        OnAnyUnitSpawned?.Invoke(this, null);
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in baseActionArray)
        {
            if (baseAction is T)
            {
                return (T) baseAction;
            }
        }
        return null;
    }
}
