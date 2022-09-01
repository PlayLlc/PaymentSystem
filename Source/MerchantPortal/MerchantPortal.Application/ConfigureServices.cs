using FluentValidation;
using MerchantPortal.Application.Contracts.Services;
using MerchantPortal.Application.Services.PoS;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MerchantPortal.Application;

public static class ConfigureServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<IPoSConfigurationService, PoSConfigurationService>();
    }
}
