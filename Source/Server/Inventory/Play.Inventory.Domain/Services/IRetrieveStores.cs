using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Services;

public interface IRetrieveStores
{
    #region Instance Members

    public Task<Store?> GetByIdAsync(string id);
    public Store? GetById(string id);

    #endregion
}