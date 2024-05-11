using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointText;
    [SerializeField] private UnitScript unit;
    [SerializeField] private Image healthBar;
    private HealthSystem healthSystem;

    void Start()
    {
        healthSystem = unit.GetComponent<HealthSystem>();
        //NOT EFFECTIVE
        //WILL BE CALLED ANYTIME
        UnitScript.OnAnyActionPointsChanged += UnitScript_OnAnyActionPointsChanged;
        healthSystem.OnDamage += HealthSystem_OnDamage;
        UpdateActionPointsText();
        UpdateHealthBar();
    }

    private void UpdateActionPointsText()
    {
        actionPointText.text = unit.GetActionPoints().ToString();
    }

    private void UpdateHealthBar()
    {
        Debug.Log(unit.GetHealthNormalized());
        healthBar.fillAmount = unit.GetHealthNormalized();
    }

    void UnitScript_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }
    
    void HealthSystem_OnDamage(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
}
