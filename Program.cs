using Microsoft.Extensions.Hosting;
using SlotMachine.Extensions;

namespace SlotMachine;

public class Program
{
    public static void Main(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
                services
                    .AddSlotMachineConfiguration(hostContext.Configuration)
                    .AddSlotMachineServices())
            .BuildSlotMachine();
}
