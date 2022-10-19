using Play.Accounts.Domain.Aggregates;
using Play.Domain.Repositories;

namespace Play.Accounts.Persistence.Sql.Repositories;

public interface IUserRepository : IRepository<User, string>
{
    #region Instance Members

    public Task<bool> IsEmailUnique(string email);

    #endregion
}