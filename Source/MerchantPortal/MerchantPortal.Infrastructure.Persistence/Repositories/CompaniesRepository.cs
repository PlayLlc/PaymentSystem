using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Core.Entities;
using MerchantPortal.Infrastructure.Persistence.Sql;

namespace MerchantPortal.Infrastructure.Persistence.Repositories;

internal class CompaniesRepository : Repository<CompanyEntity>, ICompaniesRepository
{
    public CompaniesRepository(MerchantPortalDbContext dbContext) : base(dbContext)
    {
    }
}
