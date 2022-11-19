using Play.Domain.Repositories;
using Play.Domain.Common.ValueObjects;
using Play.Inventory.Contracts.Dtos;
using Play.Inventory.Domain.Aggregates;

namespace Play.Inventory.Domain.Repositories;

public interface IItemRepository : IRepository<Item, SimpleStringId>
{
    #region Instance Members

    public Task<IEnumerable<ItemDto>> GetItemsAsync(SimpleStringId merchantId);
    public Task<IEnumerable<ItemDto>> GetItemsAsync(SimpleStringId merchantId, int pageSize, int position);

    #endregion
}