using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Core.Entities;
using MerchantPortal.Infrastructure.Persistence.Sql;

namespace MerchantPortal.Infrastructure.Persistence.Repositories;

internal class StoresRepository : Repository<StoreEntity>, IStoresRepository
{
    internal StoresRepository(MerchantPortalDbContext dbContext) : base(dbContext)
    {
    }
}
