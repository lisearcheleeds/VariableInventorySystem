namespace VariableInventory
{
    public interface IStandardCaseCellData : IVariableInventoryCellData
    {
        StandardCaseViewData CaseData { get; }
    }
}
