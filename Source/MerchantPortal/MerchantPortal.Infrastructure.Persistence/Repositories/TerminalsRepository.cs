using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Core.Entities;
using MerchantPortal.Infrastructure.Persistence.Sql;

namespace MerchantPortal.Infrastructure.Persistence.Repositories;

internal class TerminalsRepository : Repository<TerminalEntity>, ITerminalsRepository
{
    public TerminalsRepository(MerchantPortalDbContext dbContext) : base(dbContext)
    {
    }

    public IEnumerable<TerminalEntity> SelectTerminalsByStore(long storeId)
    {
        return _dbContext.Terminals.Where(x => x.StoreId == storeId);
    }
}
