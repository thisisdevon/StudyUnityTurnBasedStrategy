using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;

    public void Show()
    {
        mesh.enabled = true;
    }

    public void Hide()
    {
        mesh.enabled = false;
    }
}
