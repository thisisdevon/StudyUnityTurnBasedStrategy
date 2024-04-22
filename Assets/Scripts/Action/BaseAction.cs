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

    public virtual void ActionSelected(Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
    }

    public virtual void ActionExecute()
    {
        isActive = true;
    }

    public virtual void ActionComplete()
    {
        //UnitActionSystemScript.Instance.ClearIsRunningAction();
        isActive = false;
    }
}
