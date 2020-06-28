using System.Linq;
using UnityEngine;
using VariableInventorySystem;

namespace VariableInventorySystem.Sample
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
            ImageAsset = new VariableInventorySystem.StandardAsset("Image/chest");
            CaseData = new VariableInventorySystem.StandardCaseViewData(8, 6);
        }
    }
}