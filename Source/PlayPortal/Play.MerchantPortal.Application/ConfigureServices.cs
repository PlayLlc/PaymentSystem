using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Play.MerchantPortal.Application.Mapping;
using Play.MerchantPortal.Application.Services.Merchants;
using Play.MerchantPortal.Application.Services.Stores;
using Play.MerchantPortal.Application.Services.Terminals;
using Play.MerchantPortal.Contracts.DTO;
using Play.MerchantPortal.Contracts.Services;

namespace Play.MerchantPortal.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(PersistenceMapperProfile));

        services.AddScoped<IValidator<MerchantDto>, MerchantValidator>();
        services.AddScoped<ITerminalConfigurationService, TerminalConfigurationService>();
        services.AddScoped<IStoreConfigurationService, StoreConfigurationService>();
        services.AddScoped<IMerchantConfigurationService, MerchantConfigurationService>();

        return services;
    }
}
