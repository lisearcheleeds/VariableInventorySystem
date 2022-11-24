using System.Collections.Generic;
using UnityEngine;

namespace VariableInventorySystem
{
    public class StandardCore : VariableInventoryCore
    {

        [SerializeField] GameObject cellPrefab;
        [SerializeField] GameObject casePopupPrefab;
        [SerializeField] RectTransform effectCellParent;
        [SerializeField] RectTransform caseParent;

        protected override GameObject CellPrefab => cellPrefab;
        protected override RectTransform EffectCellParent => effectCellParent;

        protected List<IStandardCaseCellData> popupList = new List<IStandardCaseCellData>();

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

        public void RemoveInventoryItem(StandardStashView stashView)
        {
            if (stareCell.CellData is IStandardCaseCellData caseData)
            {
                Remove(stashView, caseData, true);
            }

            if (stareCell.CellData is IVariableInventoryCellData cellData)
            {
                Remove(stashView, cellData, false);
            }
        }
        
        private void Remove(StandardStashView stashView, IVariableInventoryCellData cellData, bool isCase)
        {
            int id = stashView.StashData.GetId(cellData) ?? default(int);
            if (stashView.StashData.CellData[id] is IStandardCaseCellData && !isCase)
                return; // Can't drop items inside a case
            stashView.StashData.CellData[id] = null;
            stashView.Apply(stashView.StashData);
        }
    }
}
