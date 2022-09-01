using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Core.Entities;
using MerchantPortal.Infrastructure.Persistence.Sql;
using Microsoft.EntityFrameworkCore;

namespace MerchantPortal.Infrastructure.Persistence.Repositories;

internal class StoresRepository : Repository<StoreEntity>, IStoresRepository
{
    public StoresRepository(MerchantPortalDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<StoreEntity> SelectById(long id)
    {
        return await _dbContext.Stores.AsNoTracking().FirstAsync(x => x.Id == id);
    }

    public IEnumerable<StoreEntity> SelectStoresByMerchant(long merchantId)
    {
        return _dbContext.Stores.Where(x => x.MerchantId == merchantId).AsEnumerable();
    }
}
