using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GridPosition mouseGridPosition = LevelGridScript.Instance.GetGridPosition(MouseWorldScript.GetPosition());
            GridPosition startGridPosition = new GridPosition(0, 0);

            List<GridPosition> gridPositionList = Pathfinding.Instance.FindPath(startGridPosition, mouseGridPosition);
            if (gridPositionList == null)
            {
                return;
            }
            for (int i = 0; i < gridPositionList.Count - 1; i++)
            {
                Vector3 startPosition = LevelGridScript.Instance.GetWorldPosition(gridPositionList[i]);
                startPosition.y = 0.1f;
                
                Vector3 endPosition = LevelGridScript.Instance.GetWorldPosition(gridPositionList[i + 1]);
                endPosition.y = 0.1f;
                Debug.DrawLine(
                    startPosition,
                    endPosition,
                    Color.yellow,
                    10f
                );
            }
        }
    }
}
