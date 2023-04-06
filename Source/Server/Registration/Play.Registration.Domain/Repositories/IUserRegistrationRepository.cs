using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;
using Play.Registration.Domain.Aggregates.UserRegistration;

namespace Play.Registration.Domain.Repositories;

public interface IUserRegistrationRepository : IRepository<UserRegistration, SimpleStringId>
{
    #region Instance Members

    public Task<bool> IsEmailUniqueAsync(string email);
    public Task<UserRegistration?> GetByEmailAsync(string email);

    #endregion
}