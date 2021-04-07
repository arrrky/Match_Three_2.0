public class PlayingFieldGenerator : IPlayingFieldGenerator
{
   private int rowsCount;
   private int columnsCount;
   private TileType[,] randomPlayingField;
   
   public PlayingFieldGenerator(int rowsCount, int columnsCount)
   {
      this.rowsCount = rowsCount;
      this.columnsCount = columnsCount;
      randomPlayingField = new TileType[rowsCount,columnsCount];
   }

   // Слева - направо, сверху - вниз
   public TileType[,] GetRandomPlayingField()
   {
      for (int rowNumber = 0; rowNumber < rowsCount; rowNumber++)
      {
         for (int columnNumber = 0; columnNumber < columnsCount; columnNumber++)
         {
            randomPlayingField[rowNumber, columnNumber] = GetCorrectTileType(rowNumber, columnNumber);
         }
      }
      return randomPlayingField;
   }

   private TileType GetCorrectTileType(int rowNumber, int columnNumber)
   {
      TileType correctTileType = MiscTools.GetRandomTypeOfTile();
      
      TileType excludedTileTypeRow = TileType.Default;
      
      // Только если слева от текущей плитки есть минимум 2 плитки (чтобы не выйти за границы массива)
      if (columnNumber > 1)
      {
         // Если 2 плитки слева одинаковые - нельзя спавнить такую же
         if (randomPlayingField[rowNumber, columnNumber - 1] == randomPlayingField[rowNumber, columnNumber - 2])
         {
            //Исключаем такую плитку и рандомим, пока не найдем другую
            excludedTileTypeRow = randomPlayingField[rowNumber, columnNumber - 1];

            do
            {
               correctTileType = MiscTools.GetRandomTypeOfTile();
            } while (correctTileType == excludedTileTypeRow);
         }
      }

      TileType excludedTileTypeColumn = TileType.Default;
      
      // Только если сверху от текущей плитки есть минимум 2 плитки (чтобы не выйти за границы массива)
      if (rowNumber > 1)
      {
         if (randomPlayingField[rowNumber - 1, columnNumber] == randomPlayingField[rowNumber - 2, columnNumber])
         {
            excludedTileTypeColumn = randomPlayingField[rowNumber - 1, columnNumber];
            
            //То же самое, за исключением того, что нам исключаем уже 2 типа плиток
            do
            {
               correctTileType = MiscTools.GetRandomTypeOfTile();
            } while (correctTileType == excludedTileTypeColumn || correctTileType == excludedTileTypeRow);
         }
      }

      return correctTileType;
   }
}
