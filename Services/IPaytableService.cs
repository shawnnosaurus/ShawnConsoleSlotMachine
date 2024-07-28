namespace SlotMachine.Services;

public interface IPaytableService : IDisposable
{
    /// <summary>
    /// Gets a formatted string for display.
    /// </summary>
    /// <returns>A table of the configured paytable.</returns>
    string ToString();
}
