using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private int width;
    private int height;
    private float cellSize;
    private GridObject[,] gridObjectArray;


    
    public GridSystem(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectArray = new GridObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for(int z =0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x,z] = new GridObject(this, gridPosition);
            }
        }
    }

    public struct GridPosition : IEquatable<GridPosition>
    {
        public int x;
        public int z;
        public GridPosition(int x, int z)
        {
            this.x = x;
            this.z = z;
        }

        public override string ToString()
        {
            return "x:"  + x + ",z:" + z;
        }

        public static bool operator ==(GridPosition a, GridPosition b)
        {
            return a.x == b.x && a.z == b.z;
        }

        public static bool operator !=(GridPosition a, GridPosition b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return obj is GridPosition position && x == position.x && z == position.z;
        }

        public bool Equals(GridPosition other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return x * 10000 + z;
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
                debugTransform.GetComponent<GridDebugObjectScript>().SetGridObject(GetGridObject(x, z));
            }
        }
    }

    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }


    public GridObject GetGridObject(int x, int z)
    {
        return gridObjectArray[x, z];
    }
}
