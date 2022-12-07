using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;
using Play.Identity.Domain.Aggregates;

namespace Play.Identity.Domain.Repositories;

public interface IUserRegistrationRepository : IRepository<UserRegistration, SimpleStringId>
{
    #region Instance Members

    public Task<bool> IsEmailUniqueAsync(string email);
    public Task<UserRegistration?> GetByEmailAsync(string email);

    #endregion
}