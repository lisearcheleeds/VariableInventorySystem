namespace VariableInventory
{
    public class StandardAsset : IVariableInventoryAsset
    {
        public string Path { get; }

        public StandardAsset(string path)
        {
            Path = path;
        }
    }
}
