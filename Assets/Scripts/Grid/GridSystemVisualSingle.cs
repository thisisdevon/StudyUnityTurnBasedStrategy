using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;

    public void Show(Material material)
    {
        mesh.enabled = true;
        mesh.material = material;
    }

    public void Hide()
    {
        mesh.enabled = false;
    }
}
