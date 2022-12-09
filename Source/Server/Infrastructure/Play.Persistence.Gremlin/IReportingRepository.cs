using Microsoft.Extensions.Logging;

using Play.Domain.Events;

namespace Play.Persistence.Sql;

public interface IReportingRepository
{
    #region Instance Members

    public async Task SaveAsync(DomainEvent aggregate)
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

    #endregion
}