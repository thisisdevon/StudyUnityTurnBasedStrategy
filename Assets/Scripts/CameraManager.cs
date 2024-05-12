using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;

    void Start()
    {
        BaseAction.OnAnyActionExecuted += BaseAction_OnAnyActionExecuted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
        HideActionCamera();
    }

    private void BaseAction_OnAnyActionExecuted(object sender, EventArgs empty)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                UnitScript targetUnit = shootAction.GetTargetUnit();
                UnitScript shooterUnit = shootAction.GetOwnerUnit();

                float shoulderOffsetAmount = 0.5f;
                Vector3 cameraHeight = Vector3.up * 1.7f;
                Vector3 shootDir = (targetUnit.transform.position - shooterUnit.transform.position).normalized;
                Vector3 shoulderOffsetPos = Quaternion.Euler(0, 90f, 0) * shootDir * shoulderOffsetAmount;
                Vector3 cameraPosition = 
                    shooterUnit.GetWorldPosition() + 
                    cameraHeight + 
                    shoulderOffsetPos + 
                    (shootDir * -1);
                actionCameraGameObject.transform.position = cameraPosition;
                actionCameraGameObject.transform.LookAt(targetUnit.transform);
                ShowActionCamera();
                break;
        }
    }
    
    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs empty)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }

    void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }

    void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }

}
