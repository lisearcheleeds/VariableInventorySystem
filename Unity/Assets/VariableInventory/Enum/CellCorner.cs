namespace VariableInventory
{
    public enum CellCorner
    {
        None = 0,
        Top = 1 << 1,
        Bottom = 1 << 2,
        Left = 1 << 3,
        Right = 1 << 4,
        TopLeft = Top | Left,
        TopRight = Top | Right,
        BottomLeft = Bottom | Left,
        BottomRight = Bottom | Right,
    }
}
