using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGridScript : MonoBehaviour
{
    [SerializeField] private Transform gridDebugObjectPrefab;

    public static LevelGridScript Instance { get; private set; }

    private GridSystem gridSystem;
    private List<UnitScript> unitList;

    void Awake()
    {
        gridSystem = new GridSystem(10, 10, 2.0f);
        unitList = new List<UnitScript>();
        Instance = this;
    }

    void Start()
    {
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    public List<UnitScript> GetUnitsAtGridPosition(GridSystem.GridPosition gridPosition)
    {

        return GetGridObject(gridPosition).UnitList;
    }

    private void UnitScript_OnUnitMoving(object sender, UnitScript unit)
    {
        GridObject gridObject = GetUnitGridObjectFromPosition(unit);
        unit.UpdateGridObject(gridObject);
    }


    private GridObject GetGridObject(GridSystem.GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition);
    }

    private GridObject GetUnitGridObjectFromPosition(UnitScript unit)
    {
        GridSystem.GridPosition gridPosition = gridSystem.GetGridPosition(unit.transform.position);
        return GetGridObject(gridPosition);
    }

    public void AssignUnit(UnitScript unit)
    {
        unit.OnUnitMoving += UnitScript_OnUnitMoving;
        unitList.Add(unit);
        unit.UpdateGridObject(GetUnitGridObjectFromPosition(unit));
    }
}
