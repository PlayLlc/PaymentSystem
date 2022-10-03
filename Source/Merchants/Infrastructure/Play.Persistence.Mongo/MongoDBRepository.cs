using System.Linq.Expressions;

using MongoDB.Driver;

using Play.Domain.Aggregates;
using Play.Domain.Entities;
using Play.Domain.Repositories;

namespace Play.Persistence.Mongo;

public class MongoDbRepository<_Aggregate, _TId> : IRepository<_Aggregate, _TId> where _Aggregate : Aggregate<_TId>
{
    #region Instance Values

    private readonly IMongoDatabase _MongoDatabase;

    private string _CollectionName => typeof(_Aggregate).Name;

    #endregion

    #region Constructor

    public MongoDbRepository(IMongoDatabase mongoDatabase)
    {
        _MongoDatabase = mongoDatabase;
    }

    #endregion

    #region Async

    /// <exception cref="MongoRepositoryException"></exception>
    public async Task<IEnumerable<_Aggregate>> GetAllAsync(Expression<Func<_Aggregate, bool>> predicate)
    {
        try
        {
            IAsyncCursor<_Aggregate>? cursor = await _MongoDatabase.GetCollection<_Aggregate>(_CollectionName).FindAsync(predicate).ConfigureAwait(false);

            return cursor.ToEnumerable();
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error retrieving all {nameof(_Aggregate)} documents", ex);
        }
    }

    /// <exception cref="MongoRepositoryException"></exception>
    public async Task<_Aggregate> GetByIdAsync(EntityId<_TId> id)
    {
        try
        {
            return await _MongoDatabase.GetCollection<_Aggregate>(_CollectionName).Find(x => x.Id == id).SingleAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error retrieving the {nameof(_Aggregate)} document with the {nameof(EntityId<_TId>)}: [{id}]", ex);
        }
    }

    /// <exception cref="MongoRepositoryException"></exception>
    public async Task SaveAsync(_Aggregate aggregate)
    {
        try
        {
            ReplaceOneResult? result = await _MongoDatabase.GetCollection<_Aggregate>(_CollectionName)
                .ReplaceOneAsync(x => x.Id == aggregate.Id, aggregate, new ReplaceOptions() {IsUpsert = true})
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error saving the {nameof(_Aggregate)} document: [{aggregate}]", ex);
        }
    }

    /// <exception cref="MongoRepositoryException"></exception>
    public async Task RemoveAsync(EntityId<_TId> id)
    {
        try
        {
            await _MongoDatabase.GetCollection<_Aggregate>(_CollectionName).DeleteOneAsync(x => x.Id == id).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error removing the {nameof(_Aggregate)} document with the {nameof(EntityId<_TId>)}: [{id}]", ex);
        }
    }

    #endregion

    #region Synchronous

    /// <exception cref="MongoRepositoryException"></exception>
    public _Aggregate GetById(EntityId<_TId> id)
    {
        try
        {
            return _MongoDatabase.GetCollection<_Aggregate>(_CollectionName).Find(x => x.Id == id).Single();
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error retrieving the {nameof(_Aggregate)} document with the {nameof(EntityId<_TId>)}: [{id}]", ex);
        }
    }

    /// <exception cref="MongoRepositoryException"></exception>
    public void Remove(EntityId<_TId> id)
    {
        try
        {
            _MongoDatabase.GetCollection<_Aggregate>(_CollectionName).DeleteOne(x => x.Id == id);
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error removing the {nameof(_Aggregate)} document with the {nameof(EntityId<_TId>)}: [{id}]", ex);
        }
    }

    /// <exception cref="MongoRepositoryException"></exception>
    public void Save(_Aggregate aggregate)
    {
        try
        {
            Task<ReplaceOneResult>? result = _MongoDatabase.GetCollection<_Aggregate>(_CollectionName)
                .ReplaceOneAsync(x => x.Id == aggregate.Id, aggregate, new ReplaceOptions() {IsUpsert = true});
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error saving the {nameof(_Aggregate)} document: [{aggregate}]", ex);
        }
    }

    #endregion
}