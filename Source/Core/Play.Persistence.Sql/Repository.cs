using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Play.Domain.Aggregates;
using Play.Domain.Repositories;

namespace Play.Persistence.Sql;

public class Repository<_Aggregate, _TId> : IRepository<_Aggregate, _TId> where _Aggregate : Aggregate<_TId> where _TId : IEquatable<_TId>
{
    #region Instance Values

    protected readonly DbContext _DbContext;
    protected readonly DbSet<_Aggregate> _DbSet;
    private readonly ILogger<Repository<_Aggregate, _TId>> _Logger;

    #endregion

    #region Constructor

    public Repository(DbContext dbContext, ILogger<Repository<_Aggregate, _TId>> logger)
    {
        _Logger = logger;
        _DbContext = dbContext;
        _DbSet = dbContext.Set<_Aggregate>();
        dbContext.ChangeTracker.LazyLoadingEnabled = false;

        // dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
    }

    #endregion

    #region Synchronous

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public virtual _Aggregate? GetById(_TId id)
    {
        try
        {
            return _DbSet.FirstOrDefault(a => a.Id.Equals(id));
        }
        catch (Exception ex)
        {
            EntityFrameworkRepositoryException exception = new(
                $"The {nameof(Repository<_Aggregate, _TId>)} encountered an exception retrieving {nameof(_Aggregate)} with the Identifier: [{id}]", ex);
            _Logger.LogError(new EventId(id.GetHashCode(), $"{nameof(Repository<_Aggregate, _TId>)}-{nameof(_Aggregate)}-{nameof(GetById)}"), exception,
                exception.Message);

            throw exception;
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
            EntityFrameworkRepositoryException exception = new(
                $"The {nameof(Repository<_Aggregate, _TId>)} encountered an exception retrieving the {nameof(_Aggregate)}: [{aggregate}]", ex);
            _Logger.LogError(new EventId(aggregate.GetHashCode(), $"{nameof(Repository<_Aggregate, _TId>)}-{nameof(_Aggregate)}-{nameof(Remove)}"), exception,
                exception.Message);

            throw exception;
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public void Save(_Aggregate aggregate)
    {
        try
        {
            if (_DbContext.Entry(aggregate).State == EntityState.Detached)
                _DbSet.Add(aggregate);
            else
                _DbSet.Update(aggregate);
            _DbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            EntityFrameworkRepositoryException exception = new(
                $"The {nameof(Repository<_Aggregate, _TId>)} encountered an exception saving the {nameof(_Aggregate)}: [{aggregate}]", ex);
            _Logger.LogError(new EventId(aggregate.GetHashCode(), $"{nameof(Repository<_Aggregate, _TId>)}-{nameof(_Aggregate)}-{nameof(Save)}"), exception,
                exception.Message);

            throw exception;
        }
    }

    #endregion

    #region Async

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public virtual async Task<_Aggregate?> GetByIdAsync(_TId id)
    {
        try
        {
            return await _DbSet.Where(a => a.Id.Equals(id)).FirstOrDefaultAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            EntityFrameworkRepositoryException exception = new(
                $"The {nameof(Repository<_Aggregate, _TId>)} encountered an exception retrieving {nameof(_Aggregate)} with the Identifier: [{id}]", ex);
            _Logger.LogError(new EventId(id.GetHashCode(), $"{nameof(Repository<_Aggregate, _TId>)}-{nameof(_Aggregate)}-{nameof(GetByIdAsync)}"), exception,
                exception.Message);

            throw exception;
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task SaveAsync(_Aggregate aggregate)
    {
        try
        {
            if (_DbContext.Entry(aggregate).State == EntityState.Detached)
                _DbSet.Add(aggregate);
            else
                _DbSet.Update(aggregate);

            await _DbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            EntityFrameworkRepositoryException exception = new(
                $"The {nameof(Repository<_Aggregate, _TId>)} encountered an exception saving the {nameof(_Aggregate)}: [{aggregate}]", ex);
            _Logger.LogError(new EventId(aggregate.GetHashCode(), $"{nameof(Repository<_Aggregate, _TId>)}-{nameof(_Aggregate)}-{nameof(SaveAsync)}"),
                exception, exception.Message);

            throw exception;
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
            EntityFrameworkRepositoryException exception = new(
                $"The {nameof(Repository<_Aggregate, _TId>)} encountered an exception retrieving the {nameof(_Aggregate)}: [{aggregate}]", ex);
            _Logger.LogError(new EventId(aggregate.GetHashCode(), $"{nameof(Repository<_Aggregate, _TId>)}-{nameof(_Aggregate)}-{nameof(RemoveAsync)}"),
                exception, exception.Message);

            throw exception;
        }
    }

    #endregion
}