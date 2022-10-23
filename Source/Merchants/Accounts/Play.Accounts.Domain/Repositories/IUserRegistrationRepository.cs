using Play.Accounts.Domain.Aggregates;
using Play.Domain.Repositories;

namespace Play.Accounts.Domain.Repositories;

public interface IUserRegistrationRepository : IRepository<UserRegistration, string>
{
    #region Instance Members

    public Task<bool> IsEmailUniqueAsync(string email);
    public Task<UserRegistration?> GetByEmailAsync(string email);

    #endregion
}