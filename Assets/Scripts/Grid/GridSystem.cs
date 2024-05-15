using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem<TGridObject>
{
    private int width;
    private int height;
    private float cellSize;
    private TGridObject[,] gridObjectArray;


    
    public GridSystem(int width, int height, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectArray = new TGridObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for(int z =0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x,z] = createGridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize)
        );
    }

    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for(int z =0; z < height; z++)
            {
                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(x, z), Quaternion.identity);
                debugTransform.GetComponent<GridDebugObjectScript>().SetGridObject(GetGridObject(x, z) as GridObject);
            }
        }
    }

    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return GetGridObject(gridPosition.x, gridPosition.z);
    }


    public TGridObject GetGridObject(int x, int z)
    {
        if (!IsGridPositionAcceptable(x, z))
        {
            return default(TGridObject);
        }
        return gridObjectArray[x, z];
    }

    public bool IsGridPositionAcceptable(GridPosition gridPosition)
    {
        return IsGridPositionAcceptable(gridPosition.x, gridPosition.z);
    }

    public bool IsGridPositionAcceptable(int x, int z)
    {
        return x >= 0 &&  
        z >= 0 && 
        x < width && 
        z < height;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }
}
