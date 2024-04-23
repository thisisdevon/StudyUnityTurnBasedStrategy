using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private ActionButtonUI actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainer;
    [SerializeField] private TextMeshProUGUI actionPointsText;

    private List<ActionButtonUI> actionButtonList;

    void Start()
    {
        UnitActionSystemScript.Instance.OnSelectedUnitChanged += UnitActionSystemScript_OnSelectedUnitChanged;
        UnitActionSystemScript.Instance.OnSelectedActionChanged += UnitActionSystemScript_OnSelectedActionChanged;
        UnitActionSystemScript.Instance.OnActionExecute += UnitActionSystemScript_OnActionExecute;
        UnitScript.OnAnyActionPointsChanged += UnitScript_OnAnyActionPointsChanged;
        UpdateActionPointsText();
        actionButtonList = new List<ActionButtonUI>();
    }

    private void UnitActionSystemScript_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateActionPointsText();
    }

    public void UnitActionSystemScript_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    public void UnitActionSystemScript_OnActionExecute(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void UnitScript_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void CreateUnitActionButtons()
    {
        foreach(Transform transform in actionButtonContainer)
        {
            Destroy(transform.gameObject);
        }

        actionButtonList.Clear();
        actionButtonList = new List<ActionButtonUI>();

        UnitScript selectedUnit = UnitActionSystemScript.Instance.GetSelectedUnit();
        if (selectedUnit != null)
        {
            foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
            {
                ActionButtonUI actionButton = Instantiate(actionButtonPrefab, actionButtonContainer) as ActionButtonUI;
                actionButtonList.Add(actionButton);
                actionButton.SetBaseAction(baseAction);
            }
        }
    }

    public void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI actionButton in actionButtonList)
        {
            actionButton.SetSelected(actionButton.GetBaseAction() == UnitActionSystemScript.Instance.GetSelectedAction());
        }
    }

    private void UpdateActionPointsText()
    {
        string text = "";
        if (UnitActionSystemScript.Instance.GetSelectedUnit() != null)
        {
            text = "Action points: " + UnitActionSystemScript.Instance.GetSelectedUnit().GetActionPoints();
        }
        actionPointsText.text = text;
    }
}
