using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;

namespace Play.Inventory.Domain.Repositories;

public interface IInventoryRepository : IRepository<Aggregates.Inventory, SimpleStringId>
{
    #region Instance Members

    public Task<Aggregates.Inventory?> GetByStoreIdAsync(SimpleStringId storeId);
    public Task RemoveByStoreIdAsync(SimpleStringId storeId);

    #endregion
}