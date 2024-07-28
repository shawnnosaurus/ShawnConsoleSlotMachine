namespace SlotMachine.Utils;

public static class List
{
    /// <summary>
    /// Wraps overflow indexes to the start of the list.
    /// </summary>
    /// <param name="index">Lookup index</param>
    /// <returns>Value from start of the list.</returns>
    public static int GetByIndexOverflow(this List<int> list, int index) => list[index % list.Count];

    /// <summary>
    /// Converts a list to a 2d array.
    /// </summary>
    /// <param name="items"></param>
    /// <param name="colCount"></param>
    /// <param name="rowCount"></param>
    /// <returns></returns>
    public static int[,] MapAsGrid(this List<int> items, int colCount, int rowCount)
    {
        var grid = new int[rowCount, colCount];
        var index = 0;

        for (var row = 0; row < rowCount; row++)
            for (var col = 0; col < colCount; col++)
                if (index < items.Count)
                {
                    grid[row, col] = items[index];
                    index++;
                }
                else break;

        return grid;
    }

    /// <summary>
    /// Converts a 2d-array to a list of columns of int values.
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="colCount"></param>
    /// <param name="rowCount"></param>
    /// <returns></returns>
    public static List<List<int>> MapGridColumns(this int[,] grid, int colCount, int rowCount)
    {
        var cols = new List<List<int>>();

        for (var col = 0; col < colCount; col++)
        {
            var columnValues = new List<int>();
            for (var row = 0; row < rowCount; row++)
                columnValues.Add(grid[row, col]);
            cols.Add(columnValues);
        }

        return cols;
    }
}
