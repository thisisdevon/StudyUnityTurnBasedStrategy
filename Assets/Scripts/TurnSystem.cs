using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    private int turnNumber = 1;
    public event EventHandler OnTurnChanged;
    public static TurnSystem Instance { get; private set; }

    private bool isPlayerTurn = true;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Multiple TurnSystem detected");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void MoveToNextTurn()
    {
        turnNumber++;

        isPlayerTurn = !isPlayerTurn;
        OnTurnChanged?.Invoke(this, null);
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }

    public bool IsEnemyTurn()
    {
        return !isPlayerTurn;
    }
}
