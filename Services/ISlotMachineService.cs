namespace SlotMachine.Services;

public interface ISlotMachineService : IDisposable
{
    /// <summary>
    /// Entry point for the game.
    /// </summary>
    void Start();
}
