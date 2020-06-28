namespace VariableInventorySystem
{
    public interface IStandardCaseCellData : IVariableInventoryCellData
    {
        StandardCaseViewData CaseData { get; }
    }
}
