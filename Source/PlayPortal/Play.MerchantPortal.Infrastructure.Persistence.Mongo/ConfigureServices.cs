using Microsoft.Extensions.DependencyInjection;
using Play.MerchantPortal.Application.Contracts.Persistence;

namespace Play.MerchantPortal.Infrastructure.Persistence.Mongo;

public static class ConfigureServices
{
    public static void AddMongoPersistenceServices(this IServiceCollection services)
    {
        services.AddSingleton<IMongoDbHelper, MongoDbHelper>();

        services.AddScoped<IPoSRepository, PoSRepository>();
    }
}
