using Microsoft.Extensions.DependencyInjection;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

using Play.Merchants.Domain.Repositories;
using Play.Persistence.Mongo.Helpers;

namespace Play.Merchants.Persistence.Mongo;

public static class ConfigureServices
{
    #region Instance Members

    public static void AddMongoPersistenceServices(this IServiceCollection services)
    {
        //This is obsolete but mongo team marked it as obsolete too aggresively, their suggestion is to suppres this for now.
        //http://mongodb.github.io/mongo-csharp-driver/2.11/reference/bson/guidserialization/guidrepresentationmode/guidrepresentationmode/
        //https://www.mongodb.com/community/forums/t/c-guid-style-dont-work/126901/2
#pragma warning disable CS0618 // Type or member is obsolete
        BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
#pragma warning restore CS0618 // Type or member is obsolete
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        services.AddSingleton<IMongoDbHelper, MongoDbHelper>();
        services.AddScoped<IPointOfSaleRepository, PointOfSaleRepository>();
    }

    #endregion
}