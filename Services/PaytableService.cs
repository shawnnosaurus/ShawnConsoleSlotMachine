using Microsoft.Extensions.Options;
using SlotMachine.Config;

namespace SlotMachine.Services;

public class PaytableService : IPaytableService
{
    private readonly SlotMachineConfig _config;

    public PaytableService(IOptions<SlotMachineConfig> slotMachineConfig)
    {
        _config = slotMachineConfig.Value;
    }

    public string ToString()
    {
        var spacer = "--------|-------------|-------------|-------------";

        return @$" Symbol | {string.Join(" | ", _config.Paytable.OfAKind.Select(NthKind => $"{NthKind} of a kind"))}
{spacer}{string.Join(@$"
{spacer}", _config.Symbols.Select((symbol, i) => $@"
  {symbol}  |      {string.Join("     |      ", _config.Paytable.OfAKind.Select((_, j) =>
        {
            var payoutValue = _config.Paytable.Payouts[i][j].ToString();
            return payoutValue.Length == 1 ? $"{payoutValue} " : payoutValue;
        }))}"))}";
    }

    public void Dispose()
    {

    }
}
