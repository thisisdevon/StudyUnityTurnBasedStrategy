using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }


    private List<UnitScript> unitList;
    private List<UnitScript> friendlyUnitList;
    private List<UnitScript> enemyUnitList;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one UnitManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        unitList = new List<UnitScript>();
        friendlyUnitList = new List<UnitScript>();
        enemyUnitList = new List<UnitScript>();
    }

    private void Start()
    {
        UnitScript.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        UnitScript.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        UnitScript unit = sender as UnitScript;

        unitList.Add(unit);

        if (unit.IsEnemy())
        {
            enemyUnitList.Add(unit);
        } else
        {
            friendlyUnitList.Add(unit);
        }
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        UnitScript unit = sender as UnitScript;

        unitList.Remove(unit);

        if (unit.IsEnemy())
        {
            enemyUnitList.Remove(unit);
        }
        else
        {
            friendlyUnitList.Remove(unit);
        }
    }

    public List<UnitScript> GetUnitList()
    {
        return unitList;
    }

    public List<UnitScript> GetFriendlyUnitList()
    {
        return friendlyUnitList;
    }

    public List<UnitScript> GetEnemyUnitList()
    {
        return enemyUnitList;
    }
}
