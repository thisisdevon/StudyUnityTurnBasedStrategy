using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorldScript : MonoBehaviour
{
    [SerializeField] private Transform mouseCursor;
    [SerializeField] private LayerMask mouseLayerMask;

    private static MouseWorldScript instance;

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        mouseCursor.position = MouseWorldScript.GetPosition();
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHitInfo, float.MaxValue, instance.mouseLayerMask))
        {
            return raycastHitInfo.point;
        }
        return Vector3.zero;
    }
}
