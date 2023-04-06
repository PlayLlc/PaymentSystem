using Play.Registration.Domain.Entities;

namespace Play.Registration.Domain.Repositories;

public interface IPasswordRepository
{
    #region Instance Members

    public Task<IEnumerable<Password>> GetByIdAsync(string userId);

    #endregion
}