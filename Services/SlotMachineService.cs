using Microsoft.Extensions.Options;
using SlotMachine.Config;
using SlotMachine.Utils;

namespace SlotMachine.Services;

public class SlotMachineService : ISlotMachineService
{
    private readonly SlotMachineConfig _config;
    private readonly IPaytableService _paytableService;
    private readonly IMathService _mathService;

    public SlotMachineService(
        IOptions<SlotMachineConfig> slotMachineConfig,
        IPaytableService paytableService,
        IMathService mathService
    )
    {
        _config = slotMachineConfig.Value;
        _paytableService = paytableService;
        _mathService = mathService;
    }

    public void Start()
    {
        DisplayWelcomeMessage();
        StartKeyboardEventLoop();
    }

    public void Dispose()
    {
        _paytableService.Dispose();
        _mathService.Dispose();
    }

    private void DisplayWelcomeMessage()
    {
        Console.Clear();
        Console.WriteLine($"Welcome to the {_config.GameName} slot game!");
        ShowMenu();
    }

    private void StartKeyboardEventLoop()
    {
        while (true)
        {
            if (Console.KeyAvailable)
            {
                var keyInfo = Console.ReadKey(intercept: true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.P:
                        DisplayPaytable();
                        break;
                    case ConsoleKey.R:
                        DisplayWelcomeMessage();
                        break;
                    case ConsoleKey.Escape:
                        Dispose();
                        Environment.Exit(0);
                        break;
                    default:
                        Spin();
                        break;
                }
            }
        }
    }

    private void DisplayPaytable()
    {
        Console.WriteLine(_paytableService.ToString());
        ShowMenu();
    }

    private void Spin()
    {
        var stopPos = _mathService.StopPositionIndexes;
        var colCount = _config.Bands.Count;
        var rowCount = _config.RowCount;
        var screenSymbolIndexes = new List<int>();

        Console.WriteLine($"Stop Positions: {string.Join(",", stopPos)}\nScreen:");

        for (var i = 0; i < rowCount; i++)
        {
            Console.Write("\t");

            for (var j = 0; j < colCount; j++)
            {
                var currBand = _config.Bands[j];
                var symbolIndex = currBand.GetByIndexOverflow(stopPos[j] + i);

                Console.Write($"{_config.Symbols[symbolIndex]} ");
                screenSymbolIndexes.Add(symbolIndex);
            }

            Console.WriteLine();
        }

        var winnings = _mathService.CalculateWinnings(screenSymbolIndexes);

        Console.WriteLine();
        if (winnings.Total > 0) Console.WriteLine($"Total wins: {winnings.Total}");

        winnings.WayWins.ForEach(wayWin =>
            Console.WriteLine($"- Ways win {string.Join("-", wayWin.ScreenIndexes)}, {_config.Symbols[wayWin.SymbolIndex]} x{wayWin.OfKind} {wayWin.Value}"));

        ShowMenu();
    }

    private static void ShowMenu()
    {
        Console.WriteLine("\nPress [P] for playtable, [R] to restart, [Esc] to stop, else any other key to spin...");
        Console.WriteLine();
    }
}
