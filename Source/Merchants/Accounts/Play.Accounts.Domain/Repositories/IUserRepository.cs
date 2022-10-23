using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Entities;
using Play.Domain.Repositories;

namespace Play.Accounts.Domain.Repositories;

public interface IUserRepository : IRepository<User, string>
{
    #region Instance Members

    public Task<bool> IsEmailUnique(string email);

    public Task UpdateUserRoles(string userId, params UserRole[] roles);

    #endregion
}