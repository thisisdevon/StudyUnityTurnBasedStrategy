using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystemScript : MonoBehaviour
{
    [SerializeField] private LayerMask unitLayerMask;
    private static UnitActionSystemScript instance;
    private UnitScript selectedUnit;

    void Awake()
    {
        instance = this;
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
                if (!selectedUnit.IsMoving())
                {
                    selectedUnit.Move(MouseWorldScript.GetPosition());
                    selectedUnit = null;
                }
            }
        }
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHitInfo, float.MaxValue, instance.unitLayerMask))
        {
            if (raycastHitInfo.transform.TryGetComponent<UnitScript>(out UnitScript unitHit))
            {
                instance.selectedUnit = unitHit;
                return true;
            }
        }
        return false;
    }
}
