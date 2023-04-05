using Play.Identity.Domain.Entities;

namespace Play.Identity.Domain.Repositories;

public interface IPasswordRepository
{
    #region Instance Members

    public Task<IEnumerable<Password>> GetByIdAsync(string userId);

    #endregion
}