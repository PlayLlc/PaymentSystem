using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Play.Domain.Common.ValueObjects;
using Play.Persistence.Sql;
using Play.TimeClock.Domain.Aggregates;
using Play.TimeClock.Domain.Repositories;

namespace Play.TimeClock.Persistence.Sql.Repositories;

public class EmployeeRepository : Repository<Employee, SimpleStringId>, IEmployeeRepository
{
    #region Constructor

    public EmployeeRepository(DbContext dbContext, ILogger<EmployeeRepository> logger) : base(dbContext, logger)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public override async Task<Employee?> GetByIdAsync(SimpleStringId id)
    {
        try
        {
            return await _DbSet.Include("_TimeEntries").Include("_TimePuncher").FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(EmployeeRepository)} encountered an exception attempting to invoke {nameof(GetByIdAsync)} for the {nameof(id)}: [{id}];", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public override Employee? GetById(SimpleStringId id)
    {
        try
        {
            return _DbSet.Include("_TimeEntries").Include("_TimePuncher").FirstOrDefault(a => a.Id == id);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(EmployeeRepository)} encountered an exception attempting to invoke {nameof(GetById)} for the {nameof(id)}: [{id}];", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public bool DoesEmployeeAlreadyExist(SimpleStringId merchantId, SimpleStringId userId)
    {
        try
        {
            return _DbSet.Any(a => (EF.Property<SimpleStringId>(a, "_MerchantId") == merchantId) && (EF.Property<SimpleStringId>(a, "_UserId") == userId));
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(EmployeeRepository)} encountered an exception attempting to invoke {nameof(DoesEmployeeAlreadyExist)} for the {nameof(merchantId)}: [{merchantId}] and the {nameof(userId)}: [{userId}];",
                ex);
        }
    }

    #endregion
}