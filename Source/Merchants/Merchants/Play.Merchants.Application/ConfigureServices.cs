using System.Reflection;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using Play.Merchants.Application.Mapping;
using Play.Merchants.Application.Services;
using Play.Merchants.Contracts.Services;

namespace Play.Merchants.Application;

public static class ConfigureServices
{
    #region Instance Members

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MerchantConfigurationMapperProfile));
        services.AddAutoMapper(typeof(PosConfigurationProfileMapper));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<ITerminalConfigurationService, TerminalConfigurationService>();
        services.AddScoped<IStoreConfigurationService, StoreConfigurationService>();
        services.AddScoped<IMerchantConfigurationService, MerchantConfigurationService>();
        services.AddScoped<IPointOfSaleConfigurationService, PointOfSaleConfigurationService>();

        return services;
    }

    #endregion
}