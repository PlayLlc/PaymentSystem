using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Core.Entities;
using MerchantPortal.Infrastructure.Persistence.Sql;

namespace MerchantPortal.Infrastructure.Persistence.Repositories;

internal class MerchantsRepository : Repository<MerchantEntity>, IMerchantsRepository
{
    internal MerchantsRepository(MerchantPortalDbContext dbContext) : base(dbContext)
    {
    }
}
