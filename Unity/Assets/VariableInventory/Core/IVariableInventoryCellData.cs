namespace VariableInventory
{
    public interface IVariableInventoryCellData
    {
        int Id { get; }
        int Width { get; }
        int Height { get; }
        bool IsRotate { get; set; }
        IVariableInventoryAsset ImageAsset { get; }
    }
}
