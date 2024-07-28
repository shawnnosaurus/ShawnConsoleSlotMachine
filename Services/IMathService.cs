using SlotMachine.DTO;

namespace SlotMachine.Services;

public interface IMathService : IDisposable
{
    /// <summary>
    /// Gets the index of each stop position of the configured reels.
    /// </summary>
    int[] StopPositionIndexes { get; }

    /// <summary>
    /// Gets the winnings based off the configured payouts.
    /// </summary>
    /// <param name="grid">The symbol indexes displayed on screen.</param>
    /// <returns>Returns the wiinings of the current play.</returns>
    Winnings CalculateWinnings(List<int> screenSymbolIndexes);
}
