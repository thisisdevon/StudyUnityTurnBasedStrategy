using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisualScript : MonoBehaviour
{
    private UnitScript OwnerUnit;

    private MeshRenderer  mesh;

    void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
        OwnerUnit = GetComponentInParent<UnitScript>();
        mesh.enabled = false;
    }

    void Start()
    {
        UnitActionSystemScript.Instance.OnSelectedUnitChanged += UnitActionSystemScript_OnSelectedUnitChanged;
        UpdateVisual();
    }

    private void UnitActionSystemScript_OnSelectedUnitChanged(object sender, EventArgs empty)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (OwnerUnit == null) return;
        if (OwnerUnit == UnitActionSystemScript.Instance.GetSelectedUnit())
        {
            mesh.enabled = true;
        }
        else
        {
            mesh.enabled = false;
        }
    }

    private void OnDestroy()
    {
        UnitActionSystemScript.Instance.OnSelectedUnitChanged -= UnitActionSystemScript_OnSelectedUnitChanged;
    }
}
