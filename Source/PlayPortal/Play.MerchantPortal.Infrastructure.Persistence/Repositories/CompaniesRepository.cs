using MerchantPortal.Core.Entities;
using MerchantPortal.Infrastructure.Persistence.Sql;
using Play.MerchantPortal.Application.Contracts.Persistence;

namespace MerchantPortal.Infrastructure.Persistence.Repositories;

internal class CompaniesRepository : Repository<CompanyEntity>, ICompaniesRepository
{
    public CompaniesRepository(MerchantPortalDbContext dbContext) : base(dbContext)
    {
    }
}
