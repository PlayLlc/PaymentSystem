using Play.Accounts.Domain.Aggregates.UserRegistration;
using Play.Accounts.Domain.Repositories;

namespace Play.Accounts.Persistence.Sql.Repositories;

public class UserRegistrationRepository : IUserRegistrationRepository
{
    #region Instance Members

    public Task<UserRegistration?> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(UserRegistration aggregate)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(UserRegistration id)
    {
        throw new NotImplementedException();
    }

    public UserRegistration? GetById(string id)
    {
        throw new NotImplementedException();
    }

    public void Save(UserRegistration aggregate)
    {
        throw new NotImplementedException();
    }

    public void Remove(UserRegistration entity)
    {
        throw new NotImplementedException();
    }

    #endregion
}