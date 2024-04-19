using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingGridSystem : MonoBehaviour
{
    private GridSystem gridSystem;

    [SerializeField] private Transform gridDebugObjectPrefab;
    // Start is called before the first frame update
    void Start()
    {
        gridSystem = new GridSystem(10, 10, 2.0f);
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    private void Update()
    {
        Debug.Log(gridSystem.GetGridPosition(MouseWorldScript.GetPosition()));
    }
}
