using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObjectScript : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMesh;
    private GridObject gridObject;

    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }

    private void Update()
    {
        textMesh.text = gridObject.ToString();
    }
}
