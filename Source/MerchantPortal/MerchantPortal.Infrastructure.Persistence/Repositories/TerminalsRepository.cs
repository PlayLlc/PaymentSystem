using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Core.Entities;
using MerchantPortal.Infrastructure.Persistence.Sql;

namespace MerchantPortal.Infrastructure.Persistence.Repositories;

internal class TerminalsRepository : Repository<TerminalEntity>, ITerminalsRepository
{
    internal TerminalsRepository(MerchantPortalDbContext dbContext) : base(dbContext)
    {
    }
}
