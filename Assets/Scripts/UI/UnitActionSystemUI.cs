using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private ActionButtonUI actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainer;

    void Start()
    {
        UnitActionSystemScript.Instance.OnSelectedUnitChanged += UnitActionSystemScript_CreateUnitActionButtons;
    }

    private void CreateUnitActionButtons()
    {
        foreach(Transform transform in actionButtonContainer)
        {
            Destroy(transform.gameObject);
        }

        UnitScript selectedUnit = UnitActionSystemScript.Instance.GetSelectedUnit();
        if (selectedUnit != null)
        {
            foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
            {
                ActionButtonUI actionButton = Instantiate(actionButtonPrefab, actionButtonContainer) as ActionButtonUI;
                actionButton.SetBaseAction(baseAction);
            }
        }
    }


    private void UnitActionSystemScript_CreateUnitActionButtons(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
    }
}
