using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Infrastructure.Persistence.Mongo.MongoDBHelper;
using MerchantPortal.Infrastructure.Persistence.Repositories;
using MerchantPortal.Infrastructure.Persistence.Sql;
using MerchantPortal.Infrastructure.Persistence.Sql.DataSeed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MerchantPortal.Infrastructure.Persistence;

public static class ConfigureServices
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<MerchantPortalDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddTransient<MerchantPortalDbContextDataSeed>();

        services.AddSingleton<IMongoDBHelper, MongoDBHelper>();

        services.AddScoped<IMerchantsRepository, MerchantsRepository>();
        services.AddScoped<IStoresRepository, StoresRepository>();
        services.AddScoped<ITerminalsRepository, TerminalsRepository>();
        services.AddScoped<ICompaniesRepository, CompaniesRepository>();

        return services;
    }
}
