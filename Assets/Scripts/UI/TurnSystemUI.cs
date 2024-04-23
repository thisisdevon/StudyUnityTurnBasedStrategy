using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private Button endTurnButton;
    [SerializeField] private Transform enemyTurnBanner;
    // Start is called before the first frame update
    void Start()
    {
        UpdateTurnNumber();
        UpdateEnemyTurnBanner();
        UpdateEndTurnButton();
        endTurnButton.onClick.AddListener(() => {
            TurnSystem.Instance.MoveToNextTurn();
        });
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnNumber();
        UpdateEnemyTurnBanner();
    }

    private void UpdateTurnNumber()
    {
        turnNumberText.text = "Turn: " + TurnSystem.Instance.GetTurnNumber();
    }

    private void UpdateEnemyTurnBanner()
    {
        enemyTurnBanner.gameObject.SetActive(TurnSystem.Instance.IsEnemyTurn());
    }

    private void UpdateEndTurnButton()
    {
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}
