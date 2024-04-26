using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridSystem gridSystem;
    private List<UnitScript> unitList;

    public List<UnitScript> UnitList
    {
        get { return unitList; }
        private set { unitList = value; }
    }
    private GridSystem.GridPosition gridPosition;

    public GridObject(GridSystem gridSystem, GridSystem.GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        unitList = new List<UnitScript>();
    }

    public override string ToString()
    {
        string returnString = gridPosition.ToString();
        if (unitList != null && unitList.Count > 0)
        {
            foreach(UnitScript unit in unitList)
            {
                returnString += "\n";
                returnString += unit.gameObject.name;
            }
        }
        return returnString;
    }

    public void UnitEnterGrid(UnitScript unitEnter)
    {
        if (!unitList.Contains(unitEnter))
        {
            unitList.Add(unitEnter);
        }
    }

    public void UnitLeftGrid(UnitScript unitLeft)
    {
        if (unitList.Contains(unitLeft))
        {
            unitList.Remove(unitLeft);
        }
    }

    public UnitScript GetUnit()
    {
        return unitList.Count > 0 ? unitList[0] : null;
    }
}

