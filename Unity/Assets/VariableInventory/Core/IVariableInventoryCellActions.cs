using System;

namespace VariableInventory
{
    public interface IVariableInventoryCellActions
    {
        void SetActive(bool value);
        void SetCallback(
            Action onPointerClick,
            Action onPointerClickOption,
            Action onPointerEnter,
            Action onPointerExit,
            Action onPointerDown,
            Action onPointerUp);
    }
}