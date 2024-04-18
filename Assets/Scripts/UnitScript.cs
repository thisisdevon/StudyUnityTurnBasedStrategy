using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    private Vector3 moveEndPosition;
    private Vector3 moveStartPosition;
    private float moveTime = -1f;
    private float elapsedTime = 0f;
    private float moveSpeed = 4f;
    [SerializeField] AnimationCurve moveCurve;

    private void Move(Vector3 targetPosition)
    {
        this.moveEndPosition = targetPosition;
        this.moveStartPosition = transform.position;

        elapsedTime = 0f;

        moveTime = Vector3.Distance(this.moveEndPosition, this.moveStartPosition) / moveSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        moveTime = -1f;
        elapsedTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsMoving())
        {
            if (Input.GetMouseButtonDown(0))
            {
                Move(MouseWorldScript.GetPosition());
            }
        }
        else
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / moveTime;

            float curveEvaluate = moveCurve.Evaluate(normalizedTime);
            Vector3 newPosition = Vector3.Lerp(this.moveStartPosition, this.moveEndPosition, curveEvaluate);
            transform.position = newPosition;
        }
    }

    bool IsMoving()
    {
        return elapsedTime < moveTime;
    }
}
