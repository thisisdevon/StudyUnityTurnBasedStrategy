using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGridScript : MonoBehaviour
{
    [SerializeField] private Transform gridDebugObjectPrefab;

    public static LevelGridScript Instance { get; private set; }

    private GridSystem gridSystem;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Multiple LevelGridScript detected");
            Destroy(gameObject);
            return;
        }
        gridSystem = new GridSystem(10, 10, 2.0f);
        Instance = this;
    }

    void Start()
    {
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    public List<UnitScript> GetUnitsAtGridPosition(GridSystem.GridPosition gridPosition) => GetGridObject(gridPosition)?.UnitList;

    public UnitScript GetUnitAtGridPosition(GridSystem.GridPosition gridPosition) => GetGridObject(gridPosition)?.GetUnit();


    public GridSystem.GridPosition GetGridPosition(Vector3 position) => gridSystem.GetGridPosition(position);
    public Vector3 GetWorldPosition(GridSystem.GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);
    public Vector3 GetWorldPosition(int x, int z) => gridSystem.GetWorldPosition(x, z);

    public GridObject GetGridObject(GridSystem.GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition);

    private GridObject GetUnitGridObjectFromPosition(UnitScript unit)
    {
        GridSystem.GridPosition gridPosition = gridSystem.GetGridPosition(unit.transform.position);
        return GetGridObject(gridPosition);
    }

    public void UnitChangedGridPosition(UnitScript unit, GridSystem.GridPosition oldGridPosition, GridSystem.GridPosition newGridPosition)
    {
        GetGridObject(oldGridPosition).UnitLeftGrid(unit);
        GetGridObject(newGridPosition).UnitEnterGrid(unit);
    }

    public bool IsUnitOnGridPosition(GridSystem.GridPosition gridPosition)
    {
        return GetUnitsAtGridPosition(gridPosition)?.Count > 0;
    }

    public bool IsValidGridPosition(GridSystem.GridPosition gridPosition) => gridSystem.IsGridPositionAcceptable(gridPosition);
    public bool IsValidGridPosition(int x, int z) => gridSystem.IsGridPositionAcceptable(x, z);

    public int GetGridSystemWidth() => gridSystem.GetWidth();
    public int GetGridSystemHeight() => gridSystem.GetHeight();

    public void RemoveUnitAtGridPosition(GridSystem.GridPosition  gridPosition, UnitScript unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.UnitLeftGrid(unit);
    }

}
