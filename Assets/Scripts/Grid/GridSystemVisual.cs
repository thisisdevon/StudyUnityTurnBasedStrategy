using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType type;
        public Material material;
    }
    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        Yellow,
        WhiteSoft,
        BlueSoft,
        RedSoft,
        YellowSoft
    }
    
    [SerializeField] private GridSystemVisualSingle singlePrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridMaterialList;

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

    public static GridSystemVisual Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Multiple GridSystemVisual detected");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[
            LevelGridScript.Instance.GetGridSystemWidth(), 
            LevelGridScript.Instance.GetGridSystemHeight()
        ]; 
        for (int x = 0; x < LevelGridScript.Instance.GetGridSystemWidth(); x++)
        {
            for (int z = 0; z < LevelGridScript.Instance.GetGridSystemHeight(); z++)
            {
                GridSystemVisualSingle gridVisualSingleTransform = Instantiate(singlePrefab, 
                    LevelGridScript.Instance.GetWorldPosition(x, z), 
                    Quaternion.identity) as GridSystemVisualSingle;
                if (gridVisualSingleTransform != null)
                {
                    gridSystemVisualSingleArray[x,z] = gridVisualSingleTransform;
                    gridSystemVisualSingleArray[x,z].Hide();
                }
            }   
        }

        UnitActionSystemScript.Instance.OnSelectedActionChanged += UnitActionSystemScript_OnSelectedActionChanged;
    }

    private void UnitActionSystemScript_OnSelectedActionChanged(object sender, EventArgs empty)
    {
        UpdateGridVisual();
    }

    private void HideAllGridPosition()
    {
        for (int x = 0; x < LevelGridScript.Instance.GetGridSystemWidth(); x++)
        {
            for (int z = 0; z < LevelGridScript.Instance.GetGridSystemHeight(); z++)
            {
                gridSystemVisualSingleArray[x,z].Hide();
            }   
        }
    }

    private void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType visualType)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(visualType));
        }
    }

    public void UpdateGridVisual()
    {
        HideAllGridPosition();

        BaseAction selectedAction = UnitActionSystemScript.Instance.GetSelectedAction();
        
        if (selectedAction != null)
        {
            ShowGridPositionList(selectedAction.GetExecutableActionGridPositionList(), selectedAction.GetExecutableGridVisualType());
            ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), selectedAction.GetValidGridVisualType());
        }
    }

    private Material GetGridVisualTypeMaterial(GridVisualType type)
    {
        foreach(GridVisualTypeMaterial gridVisualTypeMaterial in gridMaterialList)
        {
            if (type == gridVisualTypeMaterial.type)
            {
                return gridVisualTypeMaterial.material;
            }
        }
        return null;
    }
}
