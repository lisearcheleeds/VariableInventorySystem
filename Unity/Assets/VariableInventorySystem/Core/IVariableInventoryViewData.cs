using UnityEngine;

namespace VariableInventorySystem
{
    public interface IVariableInventoryViewData
    {
        bool IsDirty { get; set; }

        int? GetId(IVariableInventoryCellData cellData);
        int? GetInsertableId(IVariableInventoryCellData cellData);
        void InsertInventoryItem(int id, VariableInventorySystem.IVariableInventoryCellData cellData);
        bool CheckInsert(int id, VariableInventorySystem.IVariableInventoryCellData cellData);
    }
}