using Microsoft.Extensions.Options;
using SlotMachine.Config;
using SlotMachine.DTO;
using SlotMachine.Utils;

namespace SlotMachine.Services;

public class MathService : IMathService
{
    private readonly SlotMachineConfig _config;

    public int[] StopPositionIndexes => RandomStopPositions();

    public MathService(IOptions<SlotMachineConfig> slotMachineConfig)
    {
        _config = slotMachineConfig.Value;
    }

    public Winnings CalculateWinnings(List<int> screenSymbolIndexes)
    {
        var colCount = _config.Bands.Count;
        var rowCount = _config.RowCount;
        var winnings = new Winnings();

        var cols = screenSymbolIndexes
            .MapAsGrid(colCount, rowCount)
            .MapGridColumns(colCount, rowCount);
        var ofAKind = _config.Paytable.OfAKind;
        var indexPairings = new List<UniqueOccurrence>();

        for (var screenIndex = 0; screenIndex < screenSymbolIndexes.Count; screenIndex++)
        {
            var uniqueOccurrences = cols.Count(forecasted =>
                forecasted.Contains(screenSymbolIndexes[screenIndex]));
            if (ofAKind.First() <= uniqueOccurrences && uniqueOccurrences <= ofAKind.Last())
            {
                indexPairings.Add(new UniqueOccurrence()
                {
                    SymbolIndex = screenSymbolIndexes[screenIndex],
                    ScreenIndex = screenIndex,
                    Total = uniqueOccurrences
                });
            }
        }

        var groupedScreenIndexesBySymbolIndex = new Dictionary<int, List<int>>();

        foreach (var paired in indexPairings)
        {
            if (!groupedScreenIndexesBySymbolIndex.ContainsKey(paired.SymbolIndex))
            {
                groupedScreenIndexesBySymbolIndex[paired.SymbolIndex] = new List<int>();
            }
            groupedScreenIndexesBySymbolIndex[paired.SymbolIndex].Add(paired.ScreenIndex);
        }

        foreach (var kvp in groupedScreenIndexesBySymbolIndex)
        {
            var actualOccurrences = kvp.Value.Distinct().Count();

            if (actualOccurrences >= ofAKind.First())
            {
                var payoutValue = _config.Paytable.Payouts[kvp.Key][ofAKind.FindIndex(oak => oak == actualOccurrences)];

                winnings.WayWins.Add(new WayWin()
                {
                    ScreenIndexes = kvp.Value,
                    SymbolIndex = kvp.Key,
                    Value = payoutValue,
                    OfKind = actualOccurrences
                });
            }
        }

        winnings.Total = winnings.WayWins.Sum(win => win.Value);

        return winnings;
    }

    public void Dispose()
    {
    }

    private int[] RandomStopPositions()
    {
        var colCount = _config.Bands.Count;

        var rand = new Random();
        var results = new int[colCount];

        for (int i = 0; i < colCount; i++)
        {
            var currBand = _config.Bands[i];
            var bandIndex = rand.Next(currBand.Count);
            results[i] = bandIndex;
        }

        return results;
    }
}
