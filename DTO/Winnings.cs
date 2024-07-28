namespace SlotMachine.DTO;

public class Winnings
{
    public Winnings() { }

    public Winnings(int total, List<WayWin> wayWins)
    {
        Total = total;
        WayWins = wayWins;
    }

    public int Total { get; set; }
    public List<WayWin> WayWins { get; set; } = new List<WayWin>();
}
