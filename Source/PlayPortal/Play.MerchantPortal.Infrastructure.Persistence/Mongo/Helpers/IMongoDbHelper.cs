using System.Linq.Expressions;

namespace Play.MerchantPortal.Infrastructure.Persistence.Mongo;

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