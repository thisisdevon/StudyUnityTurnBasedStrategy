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
                selectedUnit = null;
                ClearIsRunningAction();
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
                if (!selectedUnit.IsMoving())
                {
                    GridSystem.GridPosition gridPosition = LevelGridScript.Instance.GetGridPosition(MouseWorldScript.GetPosition());

                    if (LevelGridScript.Instance.IsValidGridPosition (gridPosition) && selectedUnit.Move(gridPosition, ClearIsRunningAction))
                    {
                        // Start Moving
                        SetIsRunningAction();
                        hasStartedMoving = true;
                        //Instance.selectedUnit = null;
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            SetIsRunningAction();
            selectedUnit.GetSpinAction().StartSpinning(ClearIsRunningAction);
        }
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

    private void SetSelectedUnit(UnitScript unitSelected)
    {
        selectedUnit = unitSelected;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public UnitScript GetSelectedUnit()
    {
        return Instance.selectedUnit;
    }

    public bool HasUnitStoppedMoving()
    {
        return hasStartedMoving && !selectedUnit.IsMoving();
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
