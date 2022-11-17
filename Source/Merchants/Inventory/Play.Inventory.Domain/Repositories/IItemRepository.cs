using Play.Domain.Repositories;
using Play.Domain.Common.ValueObjects;

namespace Play.Inventory.Domain.Repositories;

public interface IItemRepository : IRepository<Item, SimpleStringId>
{ }