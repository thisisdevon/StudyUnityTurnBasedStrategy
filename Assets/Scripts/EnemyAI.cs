using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    private State state;

    private float timer = 3f;

    void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    state = State.Busy;
                    if (TryToTakeEnemiesAIAction(SetStateTakingTurn))
                    {

                    }
                    else
                    {
                        state = State.WaitingForEnemyTurn;
                        TurnSystem.Instance.MoveToNextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }
    }

    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }
        timer = 1f;
        state = State.TakingTurn;
    }

    private bool TryToTakeEnemiesAIAction(Action onEnemyAiActionComplete)
    {
        foreach (UnitScript unit in UnitManager.Instance.GetEnemyUnitList())
        {
            if(TryToTakeEnemyAIAction(unit, onEnemyAiActionComplete))
            {
                return true;
            }
        }
        return false;
    }

    private bool TryToTakeEnemyAIAction(UnitScript enemyUnit, Action onEnemyAiActionComplete)
    {
        SpinAction spinAction = enemyUnit.GetSpinAction();
        spinAction.ActionSelected(onEnemyAiActionComplete);

        GridSystem.GridPosition gridPosition = enemyUnit.GetGridPosition();
        if (!LevelGridScript.Instance.IsValidGridPosition(gridPosition))
        {
            return false;
            // Start Moving
        }
        
        return spinAction.ActionExecute(gridPosition);
    }
}
