using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Services;

public interface IRetrieveInventoryItems
{
    #region Instance Members

    public Task<Variation> GetByIdAsync(string itemId, string variationId);

    #endregion
}