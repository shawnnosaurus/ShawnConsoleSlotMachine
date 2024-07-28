using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SlotMachine.Config;
using SlotMachine.Services;

namespace SlotMachine.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddSlotMachineConfiguration(
        this IServiceCollection services,
        IConfiguration configuration
    ) => services.Configure<SlotMachineConfig>(configuration.GetSection("SlotMachine"));

    public static IServiceCollection AddSlotMachineServices(this IServiceCollection services) =>
        services
            .AddSingleton<ISlotMachineService, SlotMachineService>()
            .AddSingleton<IPaytableService, PaytableService>()
            .AddSingleton<IMathService, MathService>();
}
