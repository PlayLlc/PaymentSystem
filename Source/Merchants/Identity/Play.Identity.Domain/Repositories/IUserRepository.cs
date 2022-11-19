using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;
using Play.Identity.Domain.Aggregates;
using Play.Identity.Domain.Entities;

namespace Play.Identity.Domain.Repositories;

public interface IUserRepository : IRepository<User, SimpleStringId>
{
    #region Instance Members

    public Task<bool> IsEmailUnique(string email);
    public Task<User?> GetByEmailAsync(string email);
    public Task UpdateUserRoles(SimpleStringId userId, params UserRole[] roles);

    #endregion
}