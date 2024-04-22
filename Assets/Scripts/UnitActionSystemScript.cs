using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UnitActionSystemScript : MonoBehaviour
{
    public event EventHandler OnSelectedUnitChanged;
    [SerializeField] private LayerMask unitLayerMask;
    public static UnitActionSystemScript Instance { get; private set; }
    private UnitScript selectedUnit;
    private BaseAction selectedAction;
    private bool hasStartedMoving = false;

    private bool isRunningAnAction; //isBusy



    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Multiple UnitActionSystemScript detected");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunningAnAction)
        {
            if (HasUnitStoppedMoving())
            {
                hasStartedMoving = false;
                ClearSelectedUnit();
                CompleteSelectedAction();
            }
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedUnit == null)
            {
                TryHandleUnitSelection();
            }
            else
            {
                // in grid selection mode
                if (!selectedUnit.IsMoving() && selectedAction == selectedUnit.GetMoveAction())
                {
                    GridSystem.GridPosition gridPosition = LevelGridScript.Instance.GetGridPosition(MouseWorldScript.GetPosition());

                    if (LevelGridScript.Instance.IsValidGridPosition (gridPosition) && selectedUnit.GetMoveAction().TryToSetMoveParameters(gridPosition))
                    {
                        // Start Moving
                        ExecuteSelectedAction(); // temporary change to ExecuteSelectedAction() later maybe
                        hasStartedMoving = true;
                        //Instance.selectedUnit = null;
                    }
                }
            }
        }
    }

    private void SelectSelectedAction() 
    {
        selectedAction.ActionSelected(ClearSelectedUnit);
    }

    private void ExecuteSelectedAction()
    {
        SetIsRunningAction();
        selectedAction.ActionExecute();
    }

    private void CompleteSelectedAction()
    {
        ClearIsRunningAction();
        selectedAction.ActionComplete();
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHitInfo, float.MaxValue, unitLayerMask))
        {
            if (raycastHitInfo.transform.TryGetComponent<UnitScript>(out UnitScript unitHit))
            {
                SetSelectedUnit(unitHit);
                return true;
            }
        }
        return false;
    }

    public void SetSelectedAction(BaseAction baseAction) // to be called by ActionButtonUI
    {
        selectedAction = baseAction;
        SelectSelectedAction();
    }

    private void SetSelectedUnit(UnitScript unitSelected)
    {
        selectedUnit = unitSelected;
        SetSelectedAction(unitSelected.GetMoveAction());
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    private void ClearSelectedUnit()
    {
        selectedUnit = null;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public UnitScript GetSelectedUnit()
    {
        return Instance.selectedUnit;
    }

    public bool HasUnitStoppedMoving()
    {
        return selectedAction == selectedUnit.GetMoveAction() && hasStartedMoving && !selectedUnit.IsMoving();
    }

    private void SetIsRunningAction()
    {
        this.isRunningAnAction = true;
    }

    private void ClearIsRunningAction()
    {
        this.isRunningAnAction = false;
    }
}
