using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Infrastructure.Persistence.Mongo.MongoDBHelper;
using MerchantPortal.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MerchantPortal.Infrastructure.Persistence;

public static class ConfigureServices
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        services.AddSingleton<IMongoDBHelper, MongoDBHelper>();
        services.AddScoped<IPoSRepository, PoSRepository>();

        return services;
    }
}
