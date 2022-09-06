using Microsoft.EntityFrameworkCore;

using Play.MerchantPortal.Application.Contracts.Persistence;
using Play.MerchantPortal.Domain.Entities;
using Play.MerchantPortal.Infrastructure.Persistence.Sql;

namespace Play.MerchantPortal.Infrastructure.Persistence.Repositories;

internal class StoresRepository : Repository<StoreEntity>, IStoresRepository
{
    #region Constructor

    public StoresRepository(MerchantPortalDbContext dbContext) : base(dbContext)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="OperationCanceledException"></exception>
    public async Task<StoreEntity?> SelectById(long id)
    {
        return await _DbContext.Stores.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
    }

    public IEnumerable<StoreEntity> SelectStoresByMerchant(long merchantId)
    {
        return _DbContext.Stores.Where(x => x.MerchantId == merchantId).AsEnumerable();
    }

    #endregion
}