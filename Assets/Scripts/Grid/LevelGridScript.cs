using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGridScript : MonoBehaviour
{
    [SerializeField] private Transform gridDebugObjectPrefab;

    public static LevelGridScript Instance { get; private set; }

    private GridSystem<GridObject> gridSystem;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Multiple LevelGridScript detected");
            Destroy(gameObject);
            return;
        }
        gridSystem = new GridSystem<GridObject>(10, 10, 2.0f, 
        (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        Instance = this;
    }

    void Start()
    {
        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    public List<UnitScript> GetUnitsAtGridPosition(GridPosition gridPosition) => GetGridObject(gridPosition)?.UnitList;

    public UnitScript GetUnitAtGridPosition(GridPosition gridPosition) => GetGridObject(gridPosition)?.GetUnit();


    public GridPosition GetGridPosition(Vector3 position) => gridSystem.GetGridPosition(position);
    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);
    public Vector3 GetWorldPosition(int x, int z) => gridSystem.GetWorldPosition(x, z);

    public GridObject GetGridObject(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition);

    private GridObject GetUnitGridObjectFromPosition(UnitScript unit)
    {
        GridPosition gridPosition = gridSystem.GetGridPosition(unit.transform.position);
        return GetGridObject(gridPosition);
    }

    public void UnitChangedGridPosition(UnitScript unit, GridPosition oldGridPosition, GridPosition newGridPosition)
    {
        GetGridObject(oldGridPosition).UnitLeftGrid(unit);
        GetGridObject(newGridPosition).UnitEnterGrid(unit);
    }

    public bool IsUnitOnGridPosition(GridPosition gridPosition)
    {
        return GetUnitsAtGridPosition(gridPosition)?.Count > 0;
    }

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsGridPositionAcceptable(gridPosition);
    public bool IsValidGridPosition(int x, int z) => gridSystem.IsGridPositionAcceptable(x, z);

    public int GetGridSystemWidth() => gridSystem.GetWidth();
    public int GetGridSystemHeight() => gridSystem.GetHeight();

    public void RemoveUnitAtGridPosition(GridPosition  gridPosition, UnitScript unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.UnitLeftGrid(unit);
    }

}
