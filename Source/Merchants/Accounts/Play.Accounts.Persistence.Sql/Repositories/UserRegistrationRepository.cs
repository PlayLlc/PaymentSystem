using Microsoft.EntityFrameworkCore;

using Play.Accounts.Domain.Aggregates;
using Play.Persistence.Sql;

namespace Play.Accounts.Persistence.Sql.Repositories;

public class UserRegistrationRepository : Repository<UserRegistration, string>
{
    #region Constructor

    public UserRegistrationRepository(DbContext dbContext) : base(dbContext)
    { }

    #endregion
}