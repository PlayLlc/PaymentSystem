using System.Linq.Expressions;

using MongoDB.Driver;

using Play.Domain.Aggregates;
using Play.Domain.Repositories;

namespace Play.Persistence.Mongo;

public class MongoDbRepository<_Aggregate, _TId> : IRepository<_Aggregate, _TId> where _Aggregate : Aggregate<_TId> where _TId : IEquatable<_TId>
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
    public async Task<_Aggregate?> GetByIdAsync(_TId id)
    {
        try
        {
            return await _MongoDatabase.GetCollection<_Aggregate>(_CollectionName).Find(x => x.GetId()!.Equals(id)).SingleAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error retrieving the {nameof(_Aggregate)} document with the Identifier: [{id}]", ex);
        }
    }

    /// <exception cref="MongoRepositoryException"></exception>
    public async Task SaveAsync(_Aggregate aggregate)
    {
        try
        {
            ReplaceOneResult? result = await _MongoDatabase.GetCollection<_Aggregate>(_CollectionName)
                .ReplaceOneAsync(x => x.GetId()!.Equals(aggregate.GetId()), aggregate, new ReplaceOptions() {IsUpsert = true})
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error saving the {nameof(_Aggregate)} document: [{aggregate}]", ex);
        }
    }

    /// <exception cref="MongoRepositoryException"></exception>
    public async Task RemoveAsync(_Aggregate aggregate)
    {
        try
        {
            await _MongoDatabase.GetCollection<_Aggregate>(_CollectionName).DeleteOneAsync(x => x.GetId()!.Equals(aggregate.GetId())).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error removing the {nameof(_Aggregate)} document with the Identifier: [{aggregate.GetId()}]", ex);
        }
    }

    #endregion

    #region Synchronous

    /// <exception cref="MongoRepositoryException"></exception>
    public _Aggregate? GetById(_TId id)
    {
        try
        {
            return _MongoDatabase.GetCollection<_Aggregate>(_CollectionName).Find(x => x.GetId()!.Equals(id)).Single();
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error retrieving the {nameof(_Aggregate)} document with the Identifier: [{id}]", ex);
        }
    }

    /// <exception cref="MongoRepositoryException"></exception>
    public void Remove(_Aggregate aggregate)
    {
        try
        {
            _MongoDatabase.GetCollection<_Aggregate>(_CollectionName).DeleteOne(x => x.GetId()!.Equals(aggregate.GetId()));
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error removing the {nameof(_Aggregate)} document with the Identifier: [{aggregate.GetId()}]", ex);
        }
    }

    /// <exception cref="MongoRepositoryException"></exception>
    public void Save(_Aggregate aggregate)
    {
        try
        {
            Task<ReplaceOneResult>? result = _MongoDatabase.GetCollection<_Aggregate>(_CollectionName)
                .ReplaceOneAsync(x => x.GetId()!.Equals(aggregate.GetId()), aggregate, new ReplaceOptions() {IsUpsert = true});
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error saving the {nameof(_Aggregate)} document: [{aggregate}]", ex);
        }
    }

    #endregion
}