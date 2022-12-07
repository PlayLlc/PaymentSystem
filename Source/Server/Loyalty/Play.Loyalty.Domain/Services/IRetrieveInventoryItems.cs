using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Services;

public interface IRetrieveInventoryItems
{
    #region Instance Members

    public Task<InventoryItem> GetByIdAsync(string itemId, string variationId);

    #endregion
}