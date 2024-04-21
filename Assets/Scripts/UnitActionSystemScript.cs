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
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedUnit == null)
            {
                TryHandleUnitSelection();
            }
            else
            {
                if (!Instance.selectedUnit.IsMoving())
                {
                    GridSystem.GridPosition gridPosition = LevelGridScript.Instance.GetGridPosition(MouseWorldScript.GetPosition());

                    if (LevelGridScript.Instance.IsValidGridPosition (gridPosition) && Instance.selectedUnit.Move(gridPosition))
                    {
                        Instance.selectedUnit = null;
                    }
                }
            }
        }
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHitInfo, float.MaxValue, Instance.unitLayerMask))
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
        Instance.selectedUnit = unitSelected;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public UnitScript GetSelectedUnit()
    {
        return Instance.selectedUnit;
    }
}
