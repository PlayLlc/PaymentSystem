using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;
using Play.Inventory.Domain.Aggregates;

namespace Play.Inventory.Domain.Repositories;

public interface IItemRepository : IRepository<Item, SimpleStringId>
{
    #region Instance Members

    public Task<IEnumerable<Item>> GetItemsAsync(SimpleStringId merchantId);
    public Task<IEnumerable<Item>> GetItemsAsync(SimpleStringId merchantId, SimpleStringId storeId);
    public Task<IEnumerable<Item>> GetItemsAsync(SimpleStringId merchantId, int pageSize, int position);

    public Task<IEnumerable<Item>> GetItemsWithAllLocationsSet(SimpleStringId merchant);

    #endregion
}