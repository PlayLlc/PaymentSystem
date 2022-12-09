using System.Linq.Expressions;

using Microsoft.Extensions.Configuration;

using MongoDB.Driver;

namespace Play.Persistence.Mongo.Helpers;

public class MongoDbHelper : IMongoDbHelper
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

    public async Task<IEnumerable<_>> SelectAsync<_>(string collectionName) =>
        await (await GetCollection<_>(collectionName).FindAsync(Builders<_>.Filter.Empty).ConfigureAwait(false)).ToListAsync();

    public async Task<IEnumerable<_>> SelectAsync<_>(string collectionName, Expression<Func<_, bool>> filter) =>
        await (await GetCollection<_>(collectionName).FindAsync(filter).ConfigureAwait(false)).ToListAsync();

    public async Task<long> CountAsync<_>(string collectionName, Expression<Func<_, bool>> filter) =>
        await GetCollection<_>(collectionName).CountDocumentsAsync(filter).ConfigureAwait(false);

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

        return await (await GetCollection<_>(collectionName).FindAsync(Builders<_>.Filter.Empty, findOptions).ConfigureAwait(false)).FirstOrDefaultAsync();
    }

    public async Task<_> FindOneAndUpdateAsync<_>(string collectionName, Expression<Func<_, bool>> filter, params UpdateFieldConfig<_>[] updateFieldsConfig)
    {
        return await GetCollection<_>(collectionName)
            .FindOneAndUpdateAsync(filter, Builders<_>.Update.Combine(updateFieldsConfig.Select(x => Builders<_>.Update.Set(x.Field, x.Value)).ToList()))
            .ConfigureAwait(false);
    }

    public async Task InsertAsync<_>(string collectionName, _ item)
    {
        await GetCollection<_>(collectionName).InsertOneAsync(item).ConfigureAwait(false);
    }

    public async Task ClearCollectionAsync<_>(string collectionName)
    {
        await GetCollection<_>(collectionName).DeleteManyAsync(Builders<_>.Filter.Empty).ConfigureAwait(false);
    }

    public async Task DeleteOneAsync<_>(string collectionName, Expression<Func<_, bool>> filter)
    {
        await GetCollection<_>(collectionName).DeleteOneAsync(filter).ConfigureAwait(false);
    }

    public async Task AddToSetAsync<_, _TItem>(string collectionName, Expression<Func<_, bool>> filter, AddFieldConfig<_, _TItem> fieldToAdd)
    {
        await GetCollection<_>(collectionName)
            .FindOneAndUpdateAsync(filter, Builders<_>.Update.Push(fieldToAdd.FieldDefinition, fieldToAdd.Field),
                new FindOneAndUpdateOptions<_, _TItem> {IsUpsert = true})
            .ConfigureAwait(false);
    }

    private IMongoCollection<_> GetCollection<_>(string collectionName)
    {
        MongoClient client = new(_ConnectionString);
        IMongoDatabase database = client.GetDatabase(_DatabaseName);

        return database.GetCollection<_>(collectionName);
    }

    #endregion
}