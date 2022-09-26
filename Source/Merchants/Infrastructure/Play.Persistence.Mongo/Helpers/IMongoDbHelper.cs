using System.Linq.Expressions;

namespace Play.Persistence.Mongo.Helpers;

public interface IMongoDbHelper
{
    #region Instance Members

    Task<IEnumerable<_>> SelectAsync<_>(string collectionName);

    Task<IEnumerable<_>> SelectAsync<_>(string collectionName, Expression<Func<_, bool>> filter);

    Task<long> CountAsync<_>(string collectionName, Expression<Func<_, bool>> filter);

    Task<_> SelectFirstOrDefaultAsync<_>(string collectionName, SortConfig<_>? sortConfig = null, params string[] projections);

    Task<_> FindOneAndUpdateAsync<_>(string collectionName, Expression<Func<_, bool>> filter, params UpdateFieldConfig<_>[] setConfigs);

    Task InsertAsync<_>(string collectionName, _ item);

    Task ClearCollectionAsync<_>(string collectionName);

    Task DeleteOneAsync<_>(string collectionName, Expression<Func<_, bool>> filter);

    Task AddToSetAsync<_, Titem>(string collectionName, Expression<Func<_, bool>> filter, AddFieldConfig<_, Titem> fieldToAdd);

    #endregion
}