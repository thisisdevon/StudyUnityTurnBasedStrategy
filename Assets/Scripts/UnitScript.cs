using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    private Vector3 moveEndPosition;
    private Vector3 moveStartPosition;
    private float moveTime = -1f;
    private float elapsedMoveTime = 0f;
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

    // Start is called before the first frame update
    void Start()
    {
        moveTime = -1f;
        elapsedMoveTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsMoving())
        {
            unitAnimator.SetBool("IsWalking", false);
            
        }
        else
        {
            unitAnimator.SetBool("IsWalking", true);
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
