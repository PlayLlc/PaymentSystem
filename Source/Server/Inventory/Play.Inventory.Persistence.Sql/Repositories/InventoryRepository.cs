using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Play.Domain.Common.ValueObjects;
using Play.Inventory.Domain.Repositories;
using Play.Persistence.Sql;

namespace Play.Inventory.Persistence.Sql.Repositories;

public class InventoryRepository : Repository<Domain.Aggregates.Inventory, SimpleStringId>, IInventoryRepository
{
    #region Constructor

    public InventoryRepository(DbContext dbContext, ILogger<InventoryRepository> logger) : base(dbContext, logger)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task<Domain.Aggregates.Inventory?> GetByStoreIdAsync(SimpleStringId storeId)
    {
        try
        {
            Domain.Aggregates.Inventory? result = await _DbSet.Include("_StockItems").FirstOrDefaultAsync(a => a.Id == storeId).ConfigureAwait(false);

            return result;
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(ItemRepository)} encountered an exception attempting to invoke {nameof(GetByStoreIdAsync)} for the {nameof(storeId)}: [{storeId}];",
                ex);
        }
    }

    public async Task RemoveByStoreIdAsync(SimpleStringId storeId)
    {
        try
        {
            Domain.Aggregates.Inventory? result = await GetByStoreIdAsync(storeId).ConfigureAwait(false);

            if (result is null)
                return;

            _DbSet.Remove(result);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(ItemRepository)} encountered an exception attempting to invoke {nameof(RemoveByStoreIdAsync)} for the {nameof(storeId)}: [{storeId}];",
                ex);
        }
    }

    #endregion
}