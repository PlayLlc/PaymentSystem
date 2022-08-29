using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace MerchantPortal.Infrastructure.Persistence.Mongo.MongoDBHelper;

public interface IMongoDBHelper
{
    Task<IEnumerable<T>> SelectAsync<T>(string collectionName);

    Task<T> SelectFirstOrDefaultAsync<T>(string collectionName, SortConfig<T> sortConfig = null, params string[] projections);

    Task<T> FindBeforeUpdateAsync<T>(string collectionName, Expression<Func<T, bool>> filter, params UpdateFieldConfig<T>[] setConfigs);

    Task InsertAsync<T>(string collectionName, T item);

    Task ClearCollectionAsync<T>(string collectionName);

    Task DeleteOneAsync<T>(string collectionName, Expression<Func<T, bool>> filter);
}

internal class MongoDBHelper : IMongoDBHelper
{
    private readonly string _ConectionString;
    private static readonly string _DatabaseName = "MerchantPortal";

    public MongoDBHelper(IConfiguration configuration)
    {
        _ConectionString = configuration.GetConnectionString("mongo");

    }

    public async Task<IEnumerable<T>> SelectAsync<T>(string collectionName)
    {
        return await (await GetCollection<T>(collectionName).FindAsync(Builders<T>.Filter.Empty)).ToListAsync();
    }

    public async Task<T> SelectFirstOrDefaultAsync<T>(string collectionName, SortConfig<T> sortConfig = null, params string[] projections)
    {
        FindOptions<T, T> findOptions = new FindOptions<T, T>
        {
            Projection = projections.Any() ? Builders<T>.Projection.Combine(projections.Select(x => Builders<T>.Projection.Include(x)).ToList()) : null,
            Sort = sortConfig != null ? (sortConfig.SortOrder == SortOrder.Ascending ? Builders<T>.Sort.Ascending(sortConfig.SortBy) : Builders<T>.Sort.Descending(sortConfig.SortBy)) : null
        };

        return await (await GetCollection<T>(collectionName).FindAsync(Builders<T>.Filter.Empty, findOptions)).FirstOrDefaultAsync();
    }

    public async Task<T> FindBeforeUpdateAsync<T>(string collectionName, Expression<Func<T, bool>> filter, params UpdateFieldConfig<T>[] updateFieldsConfig)
    {
        return await GetCollection<T>(collectionName).FindOneAndUpdateAsync(
            filter,
            Builders<T>.Update.Combine(updateFieldsConfig.Select(x => Builders<T>.Update.SetOnInsert(x.Field, x.Value)).ToList()),
            new FindOneAndUpdateOptions<T, T>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.Before
            });
    }

    public async Task InsertAsync<T>(string collectionName, T item)
    {
        await GetCollection<T>(collectionName).InsertOneAsync(item);
    }

    public async Task ClearCollectionAsync<T>(string collectionName)
    {
        await GetCollection<T>(collectionName).DeleteManyAsync(Builders<T>.Filter.Empty);
    }

    public async Task DeleteOneAsync<T>(string collectionName, Expression<Func<T, bool>> filter)
    {
        await GetCollection<T>(collectionName).DeleteOneAsync(filter);
    }


    private IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        MongoClient client = new MongoClient(_ConectionString);
        IMongoDatabase database = client.GetDatabase(_DatabaseName);

        return database.GetCollection<T>(collectionName);
    }
}

public class UpdateFieldConfig<T>
{
    public object Value { get; set; }

    public Expression<Func<T, object>> Field { get; set; }
}

public class SortConfig<T>
{
    public SortOrder SortOrder { get; set; }

    public Expression<Func<T, object>> SortBy { get; set; }
}

public enum SortOrder
{
    Ascending = 0,
    Descending = 1
}
