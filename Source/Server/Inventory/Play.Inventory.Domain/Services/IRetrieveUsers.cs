using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Services;

public interface IRetrieveUsers
{
    #region Instance Members

    public Task<User> GetByIdAsync(string id);
    public User GetById(string id);

    #endregion
}