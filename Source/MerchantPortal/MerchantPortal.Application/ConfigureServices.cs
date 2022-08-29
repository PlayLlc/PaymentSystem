using MerchantPortal.Application.Contracts.Services;
using MerchantPortal.Application.Services.Merchants;
using MerchantPortal.Application.Services.Stores;
using MerchantPortal.Application.Services.Terminals;
using Microsoft.Extensions.DependencyInjection;

namespace MerchantPortal.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITerminalConfigurationService, TerminalConfigurationService>();
        services.AddScoped<IStoreConfigurationService, StoreConfigurationService>();
        services.AddScoped<IMerchantConfigurationService, MerchantConfigurationService>();

        return services;
    }
}
