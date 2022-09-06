﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Play.MerchantPortal.Application.Contracts.Persistence;
using Play.MerchantPortal.Infrastructure.Persistence.Mongo;
using Play.MerchantPortal.Infrastructure.Persistence.Repositories;
using Play.MerchantPortal.Infrastructure.Persistence.Sql;
using Play.MerchantPortal.Infrastructure.Persistence.Sql.DataSeed;

namespace Play.MerchantPortal.Infrastructure.Persistence;

public static class ConfigureServices
{
    #region Instance Members

    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<MerchantPortalDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddTransient<MerchantPortalDbContextDataSeed>();

        services.AddSingleton<IMongoDbHelper, MongoDbHelper>();

        services.AddScoped<IMerchantsRepository, MerchantsRepository>();
        services.AddScoped<IStoresRepository, StoresRepository>();
        services.AddScoped<ITerminalsRepository, TerminalsRepository>();
        services.AddScoped<ICompaniesRepository, CompaniesRepository>();

        return services;
    }

    #endregion
}