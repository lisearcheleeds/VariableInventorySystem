using VariableInventorySystem;

namespace VariableInventorySystem.Sample
{
    public class ItemCellData : IVariableInventoryCellData
    {
        public int Id => 0;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool IsRotate { get; set; }
        public IVariableInventoryAsset ImageAsset { get; }

        public ItemCellData(int sampleSeed)
        {
            switch (sampleSeed)
            {
                case 0:
                    Width = 2;
                    Height = 1;
                    ImageAsset = new VariableInventorySystem.StandardAsset("Image/handgun");
                    break;
                case 1:
                    Width = 2;
                    Height = 1;
                    ImageAsset = new VariableInventorySystem.StandardAsset("Image/handgun2");
                    break;
                case 2:
                    Width = 4;
                    Height = 2;
                    ImageAsset = new VariableInventorySystem.StandardAsset("Image/rifle");
                    break;
                case 3:
                    Width = 5;
                    Height = 2;
                    ImageAsset = new VariableInventorySystem.StandardAsset("Image/sniper");
                    break;
                case 4:
                    Width = 2;
                    Height = 2;
                    ImageAsset = new VariableInventorySystem.StandardAsset("Image/submachinegun");
                    break;
                case 5:
                    Width = 2;
                    Height = 1;
                    ImageAsset = new VariableInventorySystem.StandardAsset("Image/handgun");
                    break;
                case 6:
                    Width = 2;
                    Height = 1;
                    ImageAsset = new VariableInventorySystem.StandardAsset("Image/handgun2");
                    break;
            }
        }
    }
}