using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;

    private MoveAction moveAction;
    // Start is called before the first frame update
    void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnActionExecute += MoveAction_OnActionExecute;
            moveAction.OnActionComplete += MoveAction_OnActionComplete;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShooting += ShootAction_OnShooting;
        }
    }

    private void MoveAction_OnActionExecute(object sender, EventArgs empty)
    {
        unitAnimator.SetBool("IsWalking", true);
    }

    private void MoveAction_OnActionComplete(object sender, EventArgs empty)
    {
        unitAnimator.SetBool("IsWalking", false);
    }


    private void ShootAction_OnShooting(object sender, EventArgs empty)
    {
        unitAnimator.SetTrigger("ShootTrigger");
    }
}
