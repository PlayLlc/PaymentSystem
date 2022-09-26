using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Play.MerchantPortal.Domain.Persistence;
using Play.MerchantPortal.Infrastructure.Persistence.Repositories;
using Play.MerchantPortal.Infrastructure.Persistence.Sql;
using Play.MerchantPortal.Infrastructure.Persistence.Sql.DataSeed;

namespace Play.MerchantPortal.Infrastructure.Persistence;

public static class ConfigureServices
{
    #region Instance Members

    public static IServiceCollection AddSqlPersistenceServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<MerchantPortalDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddTransient<MerchantPortalDbContextDataSeed>();

        services.AddScoped<IMerchantsRepository, MerchantsRepository>();
        services.AddScoped<IStoresRepository, StoresRepository>();
        services.AddScoped<ITerminalsRepository, TerminalsRepository>();
        services.AddScoped<ICompaniesRepository, CompaniesRepository>();

        return services;
    }

    #endregion
}