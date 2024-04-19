using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    public event EventHandler OnUnitStopMoving;
    public event EventHandler<UnitScript> OnUnitMoving;
    private Vector3 moveEndPosition;
    private Vector3 moveStartPosition;
    private float moveTime = -1f;
    private float elapsedMoveTime = 0f;
    private GridObject currentGridObject;
    const float moveSpeed = 4f;
    const float rotateSpeed = 10f;
    [SerializeField] private AnimationCurve moveCurve;
    [SerializeField] private Animator unitAnimator;

    public void Move(Vector3 targetPosition)
    {
        this.moveEndPosition = targetPosition;
        this.moveStartPosition = transform.position;

        elapsedMoveTime = 0f;

        moveTime = Vector3.Distance(this.moveEndPosition, this.moveStartPosition) / moveSpeed;
    }

    public void UpdateGridObject(GridObject gridObject)
    {
        if (this.currentGridObject == null)
        {
            this.currentGridObject = gridObject;
            this.currentGridObject.UnitEnterGrid(this);
        }
        else if (this.currentGridObject != gridObject)
        {
            this.currentGridObject.UnitLeftGrid(this);
            this.currentGridObject = gridObject;
            this.currentGridObject.UnitEnterGrid(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        moveTime = -1f;
        elapsedMoveTime = 0f;

        LevelGridScript.Instance.AssignUnit(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsMoving())
        {
            unitAnimator.SetBool("IsWalking", false);
            OnUnitStopMoving?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            unitAnimator.SetBool("IsWalking", true);
            OnUnitMoving?.Invoke(this, this);
            elapsedMoveTime += Time.deltaTime;
            float normalizedTime = elapsedMoveTime / moveTime;

            float curveEvaluate = moveCurve.Evaluate(normalizedTime);
            Vector3 newPosition = Vector3.Lerp(this.moveStartPosition, this.moveEndPosition, curveEvaluate);

            //transform.forward = Vector3.Lerp(transform.forward, (this.moveEndPosition - this.moveStartPosition).normalized, Time.deltaTime * rotateSpeed);
            Quaternion targetRotation = Quaternion.LookRotation(this.moveEndPosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);

            transform.position = newPosition;
        }
    }

    public bool IsMoving()
    {
        return elapsedMoveTime < moveTime;
    }
}
