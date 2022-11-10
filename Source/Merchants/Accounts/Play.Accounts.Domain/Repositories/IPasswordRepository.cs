using Play.Accounts.Domain.Entities;

namespace Play.Accounts.Domain.Repositories;

public interface IPasswordRepository
{
    #region Instance Members

    public Task<IEnumerable<Password>> GetByIdAsync(string userId);

    #endregion
}