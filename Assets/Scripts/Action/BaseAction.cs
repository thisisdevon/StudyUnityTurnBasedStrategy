using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitScript))]
public abstract class BaseAction : MonoBehaviour
{
    public class BaseActionParameters {  }
    protected UnitScript ownerUnit;
    protected bool isActive = false;
    protected Action onActionComplete;
    protected BaseActionParameters baseActionParameter;


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

    // Let the UnitActionSystemScript to handle the activation condition

    // to be handled by SelectSelectedAction 
    public virtual void ActionSelected(Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
    }

    // to be handled by ExecuteSelectedAction 
    public virtual void ActionExecute(BaseActionParameters baseActionParameter)
    {
        this.baseActionParameter = baseActionParameter;
        isActive = IsActionValidToBeExecuted();
    }

    // to be handled by CompletrSelectedAction 
    public virtual void ActionComplete()
    {
        isActive = false;
    }

    protected abstract bool IsActionValidToBeExecuted();
}
