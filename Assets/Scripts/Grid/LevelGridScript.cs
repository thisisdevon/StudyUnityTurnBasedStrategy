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
        if (Instance != null)
        {
            Debug.Log("Multiple LevelGridScript detected");
            Destroy(gameObject);
            return;
        }
        gridSystem = new GridSystem(10, 10, 2.0f);
        unitList = new List<UnitScript>();
        Instance = this;
    }

    void Start()
    {
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    public void AssignUnit(UnitScript unit)
    {
        unit.OnUnitMoving += UnitScript_OnUnitMoving;
        unitList.Add(unit);
    }

    public List<UnitScript> GetUnitsAtGridPosition(GridSystem.GridPosition gridPosition) => GetGridObject(gridPosition).UnitList;

    public GridSystem.GridPosition GetGridPosition(Vector3 position) => gridSystem.GetGridPosition(position);

    public GridObject GetGridObject(GridSystem.GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition);

    private GridObject GetUnitGridObjectFromPosition(UnitScript unit)
    {
        GridSystem.GridPosition gridPosition = gridSystem.GetGridPosition(unit.transform.position);
        return GetGridObject(gridPosition);
    }
    private void UnitScript_OnUnitMoving(object sender, UnitScript unit)
    {
        unit.UpdateGridPosition(gridSystem.GetGridPosition(unit.transform.position));
    }

    public void UnitChangedGridPosition(UnitScript unit, GridSystem.GridPosition oldGridPosition, GridSystem.GridPosition newGridPosition)
    {
        GetGridObject(oldGridPosition).UnitLeftGrid(unit);
        GetGridObject(newGridPosition).UnitEnterGrid(unit);
    }

}
