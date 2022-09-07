using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Play.MerchantPortal.Application.Mapping;
using Play.MerchantPortal.Application.Services.Merchants;
using Play.MerchantPortal.Application.Services.PointsOfSale;
using Play.MerchantPortal.Application.Services.Stores;
using Play.MerchantPortal.Application.Services.Terminals;
using Play.MerchantPortal.Contracts.Services;
using System.Reflection;

namespace Play.MerchantPortal.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(PersistenceMapperProfile));
        services.AddAutoMapper(typeof(PoSConfigurationProfileMapper));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<ITerminalConfigurationService, TerminalConfigurationService>();
        services.AddScoped<IStoreConfigurationService, StoreConfigurationService>();
        services.AddScoped<IMerchantConfigurationService, MerchantConfigurationService>();
        services.AddScoped<IPoSConfigurationService, PoSConfigurationService>();

        return services;
    }
}
