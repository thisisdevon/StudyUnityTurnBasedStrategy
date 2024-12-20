using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private BulletProjectile bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;

    private MoveAction moveAction;
    // Start is called before the first frame update
    void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnMoveActionExecute += MoveAction_OnActionExecute;
            moveAction.OnMoveActionComplete += MoveAction_OnActionComplete;
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


    private void ShootAction_OnShooting(object sender, ShootAction.OnShootEventArgs e)
    {
        unitAnimator.SetTrigger("ShootTrigger");

        BulletProjectile bulletProjectile = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity) as BulletProjectile;
        Vector3 targetPosition = e.targetUnit.GetWorldPosition();
        targetPosition.y = shootPointTransform.position.y;
        bulletProjectile.Setup(targetPosition);
    }
}
