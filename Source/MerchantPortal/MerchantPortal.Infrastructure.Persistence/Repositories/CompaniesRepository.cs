using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Core.Entities;

namespace MerchantPortal.Infrastructure.Persistence.Repositories;

internal class CompaniesRepository : Repository<CompanyEntity>, ICompaniesRepository
{
    internal CompaniesRepository(MerchantPortalDbContext dbContext) : base(dbContext)
    {
    }
}
