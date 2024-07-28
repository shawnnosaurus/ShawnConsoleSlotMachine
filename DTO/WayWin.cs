namespace SlotMachine.DTO;

public class WayWin
{
    public WayWin() { }

    public WayWin(List<int> screenIndexes, int symbolIndex, int value)
    {
        ScreenIndexes = screenIndexes;
        SymbolIndex = symbolIndex;
        Value = value;
    }

    public List<int> ScreenIndexes { get; set; }
    public int SymbolIndex { get; set; }
    public int Value { get; set; }
    public int OfKind { get; set; }
}
