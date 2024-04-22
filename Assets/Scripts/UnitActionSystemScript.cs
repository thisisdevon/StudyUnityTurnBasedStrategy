using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (IsBusy())
        {
            if (HasUnitStoppedMoving())
            {
                hasStartedMoving = false;
                ClearSelectedUnit();
            }
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!TryHandleUnitSelection())
            {
                ExecuteSelectedAction();
            }
        }
    }

    private void SelectSelectedAction() 
    {
        if (selectedAction != null)
        {
            selectedAction.ActionSelected(ClearSelectedUnit);
        }
    }

    private void ExecuteSelectedAction()
    {
        GridSystem.GridPosition gridPosition = LevelGridScript.Instance.GetGridPosition(MouseWorldScript.GetPosition());
        if (!LevelGridScript.Instance.IsValidGridPosition(gridPosition)) // this is only checking if the grid is movable btw
        {
            return;
            // Start Moving
        }
        ;
        if (selectedAction.ActionExecute(gridPosition))
        {
            Debug.Log("Action executed");
            hasStartedMoving = true;
            SetIsRunningAction();
        }
        else
        {
            Debug.Log("Action cannot be executed");
            hasStartedMoving = false;
        }
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHitInfo, float.MaxValue, unitLayerMask))
        {
            if (raycastHitInfo.transform.TryGetComponent<UnitScript>(out UnitScript unitHit) && selectedUnit != unitHit)
            {
                SetSelectedUnit(unitHit);
                return true;
            }
        }
        return false;
    }

    public void SetSelectedAction(BaseAction baseAction) // to be called by ActionButtonUI
    {
        if (IsBusy())
        {
            return;
        }
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
        ClearIsRunningAction();
        selectedUnit = null;
        SetSelectedAction(null);
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public UnitScript GetSelectedUnit()
    {
        return selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }

    public bool HasUnitStoppedMoving()
    {
        return selectedUnit != null && selectedAction == selectedUnit.GetMoveAction() && hasStartedMoving && !selectedUnit.IsMoving();
    }

    private void SetIsRunningAction()
    {
        this.isRunningAnAction = true;
    }

    private void ClearIsRunningAction()
    {
        this.isRunningAnAction = false;
    }

    public bool IsBusy()
    {
        return this.isRunningAnAction;
    }
}
