using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitScript))]
public abstract class BaseAction : MonoBehaviour
{
    protected UnitScript ownerUnit;
    protected bool isActive = false;
    protected Action onActionComplete;


    protected virtual void Awake()
    {
        ownerUnit = GetComponent<UnitScript>();
    }

    public bool GetIsActive()
    {
        return isActive;
    }

    public virtual string GetActionName()
    {
        return "Action";
    }
}
