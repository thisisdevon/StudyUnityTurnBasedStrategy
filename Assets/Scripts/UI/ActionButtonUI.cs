using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionTextMesh;
    [SerializeField] private Button actionButton;

    private BaseAction baseAction;

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        actionTextMesh.text = baseAction.GetActionName();

        actionButton.onClick.AddListener(() => {
            UnitActionSystemScript.Instance.SetSelectedAction(baseAction);
        });
    }
}
