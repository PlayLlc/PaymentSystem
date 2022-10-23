using Microsoft.EntityFrameworkCore;

using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Repositories;
using Play.Persistence.Sql;

namespace Play.Accounts.Persistence.Sql.Repositories;

public class UserRegistrationRepository : Repository<UserRegistration, string>, IUserRegistrationRepository
{
    #region Instance Values

    private readonly DbSet<UserRegistration> _Set;

    #endregion

    #region Constructor

    public UserRegistrationRepository(DbContext dbContext) : base(dbContext)
    {
        _Set = dbContext.Set<UserRegistration>();
    }

    #endregion

    #region Instance Members

    /// <exception cref="OperationCanceledException"></exception>
    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        return await _Set.AnyAsync(a => a.GetEmail() == email).ConfigureAwait(false);
    }

    /// <exception cref="OperationCanceledException"></exception>
    public async Task<UserRegistration?> GetByEmailAsync(string email)
    {
        return await _Set.FirstOrDefaultAsync(a => a.GetEmail() == email);
    }

    #endregion
}