using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObjectScript : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMesh;
    private object gridObject;

    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
    }

    protected virtual void Update()
    {
        textMesh.text = gridObject.ToString();
    }
}
