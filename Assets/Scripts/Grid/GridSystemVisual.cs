using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private GridSystemVisualSingle singlePrefab;

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

    public static GridSystemVisual Instance { get; private set; }


    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Multiple GridSystemVisual detected");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[
            LevelGridScript.Instance.GetGridSystemWidth(), 
            LevelGridScript.Instance.GetGridSystemHeight()
        ]; 
        for (int x = 0; x < LevelGridScript.Instance.GetGridSystemWidth(); x++)
        {
            for (int z = 0; z < LevelGridScript.Instance.GetGridSystemHeight(); z++)
            {
                GridSystemVisualSingle gridVisualSingleTransform = Instantiate(singlePrefab, 
                    LevelGridScript.Instance.GetWorldPosition(x, z), 
                    Quaternion.identity) as GridSystemVisualSingle;
                if (gridVisualSingleTransform != null)
                {
                    gridSystemVisualSingleArray[x,z] = gridVisualSingleTransform;
                    gridSystemVisualSingleArray[x,z].Hide();
                }
            }   
        }
    }

    private void HideAllGridPosition()
    {
        for (int x = 0; x < LevelGridScript.Instance.GetGridSystemWidth(); x++)
        {
            for (int z = 0; z < LevelGridScript.Instance.GetGridSystemHeight(); z++)
            {
                gridSystemVisualSingleArray[x,z].Hide();
            }   
        }
    }

    private void ShowGridPositionList(List<GridSystem.GridPosition> gridPositionList)
    {
        foreach (GridSystem.GridPosition gridPosition in gridPositionList)
        {
                gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show();
        }
    }

    public void UpdateGridVisual()
    {
        HideAllGridPosition();

        UnitScript selectedUnit = UnitActionSystemScript.Instance.GetSelectedUnit();

        if (selectedUnit != null)
        {
            ShowGridPositionList(selectedUnit.GetValidActionGridPositionList());
        }
    }
}
