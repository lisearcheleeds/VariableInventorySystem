using System.Collections;
using UnityEngine;
using VariableInventorySystem;
using VariableInventorySystem.Sample;

public class SampleScene : MonoBehaviour
{
    [SerializeField] StandardCore standardCore;
    [SerializeField] StandardStashView standardStashView;
    [SerializeField] UnityEngine.UI.Button rotateButton;

    void Awake()
    {
        standardCore.Initialize();
        standardCore.AddInventoryView(standardStashView);

        rotateButton.onClick.AddListener(standardCore.SwitchRotate);

        StartCoroutine(InsertCoroutine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            standardCore.SwitchRotate();
        }

        if (Input.GetMouseButtonUp(1))
        {
            standardCore.RemoveInventoryItem(standardStashView);
        }
    }

    IEnumerator InsertCoroutine()
    {
        var stashData = new StandardStashViewData(8, 16);

        var caseItem = new CaseCellData(0);
        stashData.InsertInventoryItem(stashData.GetInsertableId(caseItem).Value, caseItem);
        standardStashView.Apply(stashData);

        for (var i = 0; i < 20; i++)
        {
            var item = new ItemCellData(i % 6);
            stashData.InsertInventoryItem(stashData.GetInsertableId(item).Value, item);
            standardStashView.Apply(stashData);

            yield return null;
        }
    }
}
