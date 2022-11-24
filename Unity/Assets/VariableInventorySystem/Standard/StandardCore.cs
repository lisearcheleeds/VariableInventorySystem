using System.Collections.Generic;
using UnityEngine;

namespace VariableInventorySystem
{
    public class StandardCore : VariableInventoryCore
    {
        [SerializeField] StandardStashView stashView;

        [SerializeField] GameObject cellPrefab;
        [SerializeField] GameObject casePopupPrefab;
        [SerializeField] RectTransform effectCellParent;
        [SerializeField] RectTransform caseParent;

        protected override GameObject CellPrefab => cellPrefab;
        protected override RectTransform EffectCellParent => effectCellParent;

        protected List<IStandardCaseCellData> popupList = new List<IStandardCaseCellData>();

        void Awake()
        {
            stashView = GetComponentInChildren<StandardStashView>();
        }

        protected override void OnCellClick(IVariableInventoryCell cell)
        {
            if (cell.CellData is IStandardCaseCellData caseData)
            {
                if (popupList.Contains(caseData))
                {
                    return;
                }

                popupList.Add(caseData);

                var standardCaseViewPopup = Instantiate(casePopupPrefab, caseParent).GetComponent<StandardCaseViewPopup>();
                AddInventoryView(standardCaseViewPopup.StandardCaseView);

                standardCaseViewPopup.Open(
                    caseData,
                    () =>
                    {
                        RemoveInventoryView(standardCaseViewPopup.StandardCaseView);
                        Destroy(standardCaseViewPopup.gameObject);
                        popupList.Remove(caseData);
                    });
            }
        }

        protected override void OnCellOptionClick(IVariableInventoryCell cell)
        {
            if (cell.CellData is IVariableInventoryCellData cellData)
            {
                int id = stashView.StashData.GetId(cellData) ?? default(int);
                stashView.StashData.CellData[id] = null;
                stashView.Apply(stashView.StashData);
            }
        }
    }
}
