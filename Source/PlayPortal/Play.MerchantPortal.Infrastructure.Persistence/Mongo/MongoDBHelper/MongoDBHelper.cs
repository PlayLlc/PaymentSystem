using Microsoft.Extensions.Configuration;

using MongoDB.Driver;

using System.Linq.Expressions;

namespace MerchantPortal.Infrastructure.Persistence.Mongo.MongoDBHelper;

public interface IMongoDbHelper
{
    #region Instance Members

    Task<IEnumerable<_>> SelectAsync<_>(string collectionName);

    Task<_> SelectFirstOrDefaultAsync<_>(string collectionName, SortConfig<_>? sortConfig = null, params string[] projections);

    Task<_> FindBeforeUpdateAsync<_>(string collectionName, Expression<Func<_, bool>> filter, params UpdateFieldConfig<_>[] setConfigs);

    Task InsertAsync<_>(string collectionName, _ item);

    Task ClearCollectionAsync<_>(string collectionName);

    Task DeleteOneAsync<_>(string collectionName, Expression<Func<_, bool>> filter);

    #endregion
}

internal class MongoDbHelper : IMongoDbHelper
{
    #region Static Metadata

    private static readonly string _DatabaseName = "MerchantPortal";

    #endregion

    #region Instance Values

    private readonly string _ConectionString;

    #endregion

    #region Constructor

    public MongoDbHelper(IConfiguration configuration)
    {
        _ConectionString = configuration.GetConnectionString("mongo");
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
        MongoClient client = new(_ConectionString);
        IMongoDatabase database = client.GetDatabase(_DatabaseName);

        return database.GetCollection<_>(collectionName);
    }

    #endregion
}

public class UpdateFieldConfig<_>
{
    #region Instance Values

    public object? Value { get; set; }

    public Expression<Func<_, object?>>? Field { get; set; }

    #endregion
}

public class SortConfig<_>
{
    #region Instance Values

    public SortOrder SortOrder { get; set; }

    public Expression<Func<_, object>>? SortBy { get; set; }

    #endregion
}

public enum SortOrder
{
    Ascending = 0,
    Descending = 1
}