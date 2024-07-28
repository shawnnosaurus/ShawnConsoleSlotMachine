using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SlotMachine.Services;

namespace SlotMachine.Extensions;

public static class HostBuilderExtension
{
    public static void BuildSlotMachine(this IHostBuilder hostBuilder)
    {
        var host = hostBuilder.Build();
        var serviceScopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
        using var scope = serviceScopeFactory.CreateScope();

        scope
            .ServiceProvider
            .GetRequiredService<ISlotMachineService>()
            .Start();
    }
}
