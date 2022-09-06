using System.Linq.Expressions;

using Microsoft.Extensions.Configuration;

using MongoDB.Driver;

namespace Play.MerchantPortal.Infrastructure.Persistence.Mongo;

internal class MongoDbHelper : IMongoDbHelper
{
    #region Static Metadata

    private static readonly string _DatabaseName = "MerchantPortal";

    #endregion

    #region Instance Values

    private readonly string _ConnectionString;

    #endregion

    #region Constructor

    public MongoDbHelper(IConfiguration configuration)
    {
        _ConnectionString = configuration.GetConnectionString("mongo");
    }

    #endregion

    #region Instance Members

    public async Task<IEnumerable<_>> SelectAsync<_>(string collectionName)
    {
        return await (await GetCollection<_>(collectionName).FindAsync(Builders<_>.Filter.Empty)).ToListAsync();
    }

    public async Task<_> SelectFirstOrDefaultAsync<_>(string collectionName, SortConfig<_>? sortConfig = null, params string[] projections)
    {
        FindOptions<_, _> findOptions = new()
        {
            Projection = projections.Any() ? Builders<_>.Projection.Combine(projections.Select(x => Builders<_>.Projection.Include(x)).ToList()) : null,
            Sort = sortConfig != null
                ? sortConfig.SortOrder == SortOrder.Ascending
                    ? Builders<_>.Sort.Ascending(sortConfig.SortBy)
                    : Builders<_>.Sort.Descending(sortConfig.SortBy)
                : null
        };

        return await (await GetCollection<_>(collectionName).FindAsync(Builders<_>.Filter.Empty, findOptions)).FirstOrDefaultAsync();
    }

    public async Task<_> FindBeforeUpdateAsync<_>(string collectionName, Expression<Func<_, bool>> filter, params UpdateFieldConfig<_>[] updateFieldsConfig)
    {
        return await GetCollection<_>(collectionName)
            .FindOneAndUpdateAsync(filter,
                Builders<_>.Update.Combine(updateFieldsConfig.Select(x => Builders<_>.Update.SetOnInsert(x.Field, x.Value)).ToList()),
                new FindOneAndUpdateOptions<_, _> {IsUpsert = true, ReturnDocument = ReturnDocument.Before});
    }

    public async Task InsertAsync<_>(string collectionName, _ item)
    {
        await GetCollection<_>(collectionName).InsertOneAsync(item);
    }

    public async Task ClearCollectionAsync<_>(string collectionName)
    {
        await GetCollection<_>(collectionName).DeleteManyAsync(Builders<_>.Filter.Empty);
    }

    public async Task DeleteOneAsync<_>(string collectionName, Expression<Func<_, bool>> filter)
    {
        await GetCollection<_>(collectionName).DeleteOneAsync(filter);
    }

    private IMongoCollection<_> GetCollection<_>(string collectionName)
    {
        MongoClient client = new(_ConnectionString);
        IMongoDatabase database = client.GetDatabase(_DatabaseName);

        return database.GetCollection<_>(collectionName);
    }

    #endregion
}