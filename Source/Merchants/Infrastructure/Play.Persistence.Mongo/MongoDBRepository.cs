using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using MongoDB.Driver;

using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.Entities;
using Play.Domain.Repositories;
using Play.Persistence.Mongo;

namespace ES.Payments.PurchaseAdjustment.Persistence.MongoDB;

public class MongoDBRepository<_Aggregate, _TId> : IRepository<_Aggregate, _TId> where _Aggregate : AggregateBase<_TId>
{
    #region Instance Values

    private readonly IMongoDatabase mongoDatabase;

    private string CollectionName => typeof(_Aggregate).Name;

    #endregion

    #region Constructor

    public MongoDBRepository(IMongoDatabase mongoDatabase)
    {
        this.mongoDatabase = mongoDatabase;
    }

    #endregion

    #region Async

    /// <exception cref="MongoRepositoryException"></exception>
    public async Task<IEnumerable<_Aggregate>> GetAllAsync(Expression<Func<_Aggregate, bool>> predicate)
    {
        try
        {
            var cursor = await mongoDatabase.GetCollection<_Aggregate>(CollectionName).FindAsync(predicate).ConfigureAwait(false);

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
            return await mongoDatabase.GetCollection<_Aggregate>(CollectionName).Find(x => x.Id == id).SingleAsync().ConfigureAwait(false);
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
            var result = await mongoDatabase.GetCollection<_Aggregate>(CollectionName)
                .ReplaceOneAsync(x => x.Id == aggregate.Id, aggregate, new ReplaceOptions() {IsUpsert = true})
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error saving the {nameof(_Aggregate)} document: [{aggregate}]", ex);
        }
    }

    public async Task RemoveAsync(EntityId<_TId> id)
    {
        try
        {
            await mongoDatabase.GetCollection<_Aggregate>(CollectionName).DeleteOneAsync(x => x.Id == id).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error removing the {nameof(_Aggregate)} document with the {nameof(EntityId<_TId>)}: [{id}]", ex);
        }
    }

    #endregion

    #region Synchronous

    public _Aggregate GetById(EntityId<_TId> id)
    {
        try
        {
            return mongoDatabase.GetCollection<_Aggregate>(CollectionName).Find(x => x.Id == id).Single();
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error retrieving the {nameof(_Aggregate)} document with the {nameof(EntityId<_TId>)}: [{id}]", ex);
        }
    }

    public void Remove(EntityId<_TId> id)
    {
        try
        {
            mongoDatabase.GetCollection<_Aggregate>(CollectionName).DeleteOne(x => x.Id == id);
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error removing the {nameof(_Aggregate)} document with the {nameof(EntityId<_TId>)}: [{id}]", ex);
        }
    }

    public void Save(_Aggregate aggregate)
    {
        try
        {
            var result = mongoDatabase.GetCollection<_Aggregate>(CollectionName)
                .ReplaceOneAsync(x => x.Id == aggregate.Id, aggregate, new ReplaceOptions() {IsUpsert = true});
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error saving the {nameof(_Aggregate)} document: [{aggregate}]", ex);
        }
    }

    #endregion
}