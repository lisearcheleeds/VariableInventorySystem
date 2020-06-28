using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VariableInventory
{
    public class StandardCaseViewData : StandardStashViewData
    {
        public StandardCaseViewData(int capacityWidth, int capacityHeight) : base(capacityWidth, capacityHeight)
        {
        }

        public StandardCaseViewData(IVariableInventoryCellData[] cellData, int capacityWidth, int capacityHeight) : base(cellData, capacityWidth, capacityHeight)
        {
        }

        public override int? GetInsertableId(IVariableInventoryCellData cellData)
        {
            if (cellData is IStandardCaseCellData caseData)
            {
                if (caseData.CaseData == this)
                {
                    return null;
                }
            }

            return base.GetInsertableId(cellData);
        }

        public override bool CheckInsert(int id, VariableInventory.IVariableInventoryCellData cellData)
        {
            if (cellData is IStandardCaseCellData caseData)
            {
                if (caseData.CaseData == this)
                {
                    return false;
                }
            }

            return base.CheckInsert(id, cellData);
        }
    }
}
