using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Play.Domain.Common.ValueObjects;
using Play.Payroll.Domain.Aggregates;
using Play.Payroll.Domain.Repositories;
using Play.Persistence.Sql;

namespace Play.Payroll.Persistence.Sql.Repositories;

public class EmployerRepository : Repository<Employer, SimpleStringId>, IEmployerRepository
{
    #region Constructor

    public EmployerRepository(DbContext dbContext, ILogger<EmployerRepository> logger) : base(dbContext, logger)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public override async Task<Employer?> GetByIdAsync(SimpleStringId id)
    {
        try
        {
            return await _DbSet.Include("_Categories").Include("_Rewards").FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(EmployerRepository)} encountered an exception attempting to invoke {nameof(GetByIdAsync)} for the {nameof(id)}: [{id}];", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public override Employer? GetById(SimpleStringId id)
    {
        try
        {
            return _DbSet.Include("_Categories").Include("_Rewards").FirstOrDefault(a => a.Id == id);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(EmployerRepository)} encountered an exception attempting to invoke {nameof(GetById)} for the {nameof(id)}: [{id}];", ex);
        }
    }

    #endregion
}