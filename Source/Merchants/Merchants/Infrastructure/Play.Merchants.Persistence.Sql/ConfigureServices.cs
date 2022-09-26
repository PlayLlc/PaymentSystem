using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Play.Merchants.Domain.Repositories;
using Play.Merchants.Persistence.Sql.Repositories;
using Play.Merchants.Persistence.Sql.Sql;
using Play.Merchants.Persistence.Sql.Sql.DataSeed;

namespace Play.Merchants.Persistence.Sql;

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