using UnityEngine;

namespace VariableInventory
{
    public interface IVariableInventoryViewData
    {
        bool IsDirty { get; set; }

        int? GetId(IVariableInventoryCellData cellData);
        int? GetInsertableId(IVariableInventoryCellData cellData);
        void InsertInventoryItem(int id, VariableInventory.IVariableInventoryCellData cellData);
        bool CheckInsert(int id, VariableInventory.IVariableInventoryCellData cellData);
    }
}