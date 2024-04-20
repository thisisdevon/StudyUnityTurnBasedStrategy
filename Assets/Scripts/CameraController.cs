using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;

    private float zoomSpeed = 5f;
    private float moveSpeed = 45f;
    private float rotateSpeed = 100f;

    void Awake()
    {
        cinemachineTransposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        UpdateRotation();
        UpdateZoom();
    }

    private void UpdateMovement()
    {
        Vector3 inputMoveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z -= 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x += 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x -= 1f;
        }

        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position = Vector3.Lerp(transform.position, transform.position + moveVector * moveSpeed, Time.deltaTime);
    }

    private void UpdateRotation()
    {
        Vector3 rotationVector = Vector3.zero;
        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y += 1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y -= 1f;
        }

        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, transform.eulerAngles + rotationVector * rotateSpeed, Time.deltaTime);
    }

    private void UpdateZoom()
    {
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
        if (Input.mouseScrollDelta.y != 0)
        {
            targetFollowOffset.y = Mathf.Clamp(
                cinemachineTransposer.m_FollowOffset.y + Input.mouseScrollDelta.y, 
                MIN_FOLLOW_Y_OFFSET,
                MAX_FOLLOW_Y_OFFSET);
            cinemachineTransposer.m_FollowOffset = Vector3.Lerp(
                cinemachineTransposer.m_FollowOffset,
                targetFollowOffset,
                Time.deltaTime * zoomSpeed
            );
        }
    }
}
