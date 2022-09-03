using MerchantPortal.Core.Entities;
using MerchantPortal.Infrastructure.Persistence.Sql;
using Microsoft.EntityFrameworkCore;
using Play.MerchantPortal.Application.Contracts.Persistence;

namespace MerchantPortal.Infrastructure.Persistence.Repositories;

internal class MerchantsRepository : Repository<MerchantEntity>, IMerchantsRepository
{
    public MerchantsRepository(MerchantPortalDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<MerchantEntity> SelectById(long id)
    {
        return await _dbContext.Merchants.AsNoTracking().FirstAsync(x => x.Id == id);
    }
}
