using System;

namespace VariableInventorySystem
{
    public interface IVariableInventoryCellActions
    {
        void SetActive(bool value);
        void SetCallback(
            Action onPointerClick,
            Action onPointerOptionClick,
            Action onPointerEnter,
            Action onPointerExit,
            Action onPointerDown,
            Action onPointerUp);
    }
}