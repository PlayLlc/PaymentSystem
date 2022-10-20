using Microsoft.EntityFrameworkCore;

using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Repositories;
using Play.Persistence.Sql;

namespace Play.Accounts.Persistence.Sql.Repositories;

public class UserRegistrationRepository : Repository<UserRegistration, string>, IUserRegistrationRepository
{
    #region Constructor

    public UserRegistrationRepository(DbContext dbContext) : base(dbContext)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="OperationCanceledException"></exception>
    public async Task<bool> IsEmailUnique(string email)
    {
        return await _DbContext.Set<UserRegistration>().AnyAsync(a => a.GetEmail() == email).ConfigureAwait(false);
    }

    #endregion
}