namespace SlotMachine.Config;

public class SlotMachineConfig
{
    public List<List<int>> Bands { get; set; }
    public string GameName { get; set; }
    public Paytable Paytable { get; set; }
    public int RowCount { get; set; }
    public List<string> Symbols { get; set; }
}

public class Paytable
{
    public List<int> OfAKind { get; set; }
    public List<List<int>> Payouts { get; set; }
}
