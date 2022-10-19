using Play.Accounts.Domain.Aggregates;
using Play.Domain.Repositories;

namespace Play.Accounts.Persistence.Sql.Repositories;

public interface IUserRegistrationRepository : IRepository<UserRegistration, string>
{
    #region Instance Members

    public Task<bool> IsEmailUnique(string email);
    public Task<UserRegistration> GetByEmail(string email);

    #endregion
}