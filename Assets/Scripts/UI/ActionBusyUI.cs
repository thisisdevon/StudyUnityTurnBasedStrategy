using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{

    [SerializeField] private Transform containerTransform;
    void Start()
    {
        UnitActionSystemScript.Instance.OnBusyChanged += ActionBusyUI_UpdateVisibility;
    }
    // Update is called once per frame
    void ActionBusyUI_UpdateVisibility(object sender, bool visibility)
    {
        gameObject.SetActive(visibility);
    }
}
