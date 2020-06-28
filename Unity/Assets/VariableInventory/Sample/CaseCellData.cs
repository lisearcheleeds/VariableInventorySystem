using System.Linq;
using UnityEngine;
using VariableInventory;

namespace VariableInventory.Sample
{
    public class CaseCellData : IStandardCaseCellData
    {
        public int Id => 0;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool IsRotate { get; set; }
        public IVariableInventoryAsset ImageAsset { get; }

        public StandardCaseViewData CaseData { get; }

        public CaseCellData(int sampleSeed)
        {
            Width = 4;
            Height = 3;
            ImageAsset = new VariableInventory.StandardAsset("Image/chest");
            CaseData = new VariableInventory.StandardCaseViewData(8, 6);
        }
    }
}