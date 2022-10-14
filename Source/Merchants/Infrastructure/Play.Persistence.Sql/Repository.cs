﻿using Microsoft.EntityFrameworkCore;

using Play.Domain.Entities;
using Play.Domain.Repositories;
using Play.Domain.Aggregates;

namespace Play.Persistence.Sql;

public class Repository<_Aggregate, _TId> : IRepository<_Aggregate, _TId> where _Aggregate : Aggregate<_TId> where _TId : IEquatable<_TId>
{
    #region Instance Values

    protected readonly DbContext _DbContext;
    protected readonly DbSet<_Aggregate> _DbSet;

    #endregion

    #region Constructor

    public Repository(DbContext dbContext)
    {
        _DbContext = dbContext;
        _DbSet = dbContext.Set<_Aggregate>();
    }

    #endregion

    #region Synchronous

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public _Aggregate? GetById(_TId id)
    {
        try
        {
            return _DbSet.FirstOrDefault(a => a.GetId()!.Equals(id));
        }
        catch (Exception ex)
        {
            // logging
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(Repository<_Aggregate, _TId>)} encountered an exception retrieving {nameof(_Aggregate)} with the Identifier: [{id}]", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public void Remove(_Aggregate aggregate)
    {
        try
        {
            _DbSet.Remove(aggregate);
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
            _DbSet.Update(aggregate);
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
    public async Task<_Aggregate?> GetByIdAsync(_TId id)
    {
        try
        {
            return await _DbSet.Where(a => a.GetId()!.Equals(id)).FirstAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // logging
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(Repository<_Aggregate, _TId>)} encountered an exception retrieving {nameof(_Aggregate)} with the Identifier: [{id}]", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task SaveAsync(_Aggregate aggregate)
    {
        try
        {
            _DbSet.Update(aggregate);
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
            _DbSet.Remove(aggregate);
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