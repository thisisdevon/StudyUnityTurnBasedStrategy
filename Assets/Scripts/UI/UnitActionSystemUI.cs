using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private ActionButtonUI actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainer;
    public static UnitActionSystemUI Instance { get; private set; }

    private List<ActionButtonUI> actionButtonList;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Multiple UnitActionSystemUI detected");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        UnitActionSystemScript.Instance.OnSelectedUnitChanged += UnitActionSystemScript_OnSelectedUnitChanged;
        UnitActionSystemScript.Instance.OnSelectedActionChanged += UnitActionSystemScript_OnSelectedActionChanged;
        actionButtonList = new List<ActionButtonUI>();
    }

    private void UnitActionSystemScript_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
    }

    public void UnitActionSystemScript_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
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
}
