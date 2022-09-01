using MerchantPortal.Application.Contracts.Services;
using MerchantPortal.Application.Services.PoS;
using Microsoft.Extensions.DependencyInjection;

namespace MerchantPortal.Application;

public static class ConfigureServices
{
    public static void RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPoSConfigurationService, PoSConfigurationService>();
    }
}
