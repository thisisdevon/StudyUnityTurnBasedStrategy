using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool invert;
    private Transform cameraTransform;

    void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.forward = invert? cameraTransform.forward * -1 : cameraTransform.forward;
    }
}
