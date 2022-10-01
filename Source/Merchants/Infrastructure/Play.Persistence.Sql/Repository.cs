using System.Data.Common;

using Microsoft.EntityFrameworkCore;

using Play.Domain;
using Play.Domain.Entities;
using Play.Domain.Repositories;

using System.Linq.Expressions;

using Play.Domain.Aggregates;
using Play.Merchants.Persistence.Sql.Exceptions;

namespace Play.Persistence.Sql;

public class Repository<_Aggregate, _TId> : IRepository<_Aggregate, _TId> where _Aggregate : AggregateBase<_TId>
{
    #region Instance Values

    protected readonly DbContext _DbContext;
    protected readonly DbSet<Dto<_TId>> _DbSet;
    protected readonly IMapDtoToAggregate _Mapper;

    #endregion

    #region Constructor

    public Repository(IMapDtoToAggregate mapper, DbSet<Dto<_TId>> dbSet, DbContext dbContext)
    {
        _Mapper = mapper;
        _DbContext = dbContext;
        _DbSet = dbSet;
    }

    #endregion

    #region Synchronous

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public _Aggregate? GetById(EntityId<_TId> id)
    {
        try
        {
            return (_Aggregate) _Mapper.AsAggregate(_DbSet.Find(id)!);
        }
        catch (Exception ex)
        {
            // logging
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(Repository<_Aggregate, _TId>)} encountered an exception retrieving {nameof(_Aggregate)} with the {nameof(EntityId<_TId>)}: [{id}]",
                ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public void Remove(_Aggregate aggregate)
    {
        try
        {
            _DbSet.Remove(aggregate.AsDto());
            _DbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            // logging
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(Repository<_Aggregate, _TId>)} encountered an exception retrieving the {nameof(_Aggregate)}: [{aggregate}]", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public void Save(_Aggregate aggregate)
    {
        try
        {
            _DbSet.Update(aggregate.AsDto());
            _DbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            // logging
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(Repository<_Aggregate, _TId>)} encountered an exception saving the {nameof(_Aggregate)}: [{aggregate}]", ex);
        }
    }

    #endregion

    #region Async

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<_Aggregate?> GetByIdAsync(EntityId<_TId> id)
    {
        try
        {
            var aggregate = await _DbSet.FindAsync(id.Id).ConfigureAwait(false);

            if (aggregate == null)
                return null;

            return (_Aggregate) _Mapper.AsAggregate(aggregate);
        }
        catch (Exception ex)
        {
            // logging
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(Repository<_Aggregate, _TId>)} encountered an exception retrieving {nameof(_Aggregate)} with {nameof(EntityId<_TId>)}: [{id}]",
                ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task SaveAsync(_Aggregate aggregate)
    {
        try
        {
            _DbSet.Update(aggregate.AsDto());
            await _DbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // logging
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(Repository<_Aggregate, _TId>)} encountered an exception saving the {nameof(_Aggregate)}: [{aggregate}]", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task RemoveAsync(_Aggregate aggregate)
    {
        try
        {
            _DbSet.Remove(aggregate.AsDto());
            await _DbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // logging
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(Repository<_Aggregate, _TId>)} encountered an exception retrieving the {nameof(_Aggregate)}: [{aggregate}]", ex);
        }
    }

    #endregion
}