public class TileIndex
{
    private int row;
    private int column;

    #region PROPERTIES
    
    public int Row
    {
        get => row;
        set => row = value;
    }

    public int Column
    {
        get => column;
        set => column = value;
    }
    
    #endregion

    public TileIndex(int row, int column)
    {
        Row = row;
        Column = column;
    }
}
