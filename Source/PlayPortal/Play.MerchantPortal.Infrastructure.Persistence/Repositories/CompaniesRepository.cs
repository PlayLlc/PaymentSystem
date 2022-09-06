using Play.MerchantPortal.Application.Contracts.Persistence;
using Play.MerchantPortal.Domain.Entities;
using Play.MerchantPortal.Infrastructure.Persistence.Sql;

namespace Play.MerchantPortal.Infrastructure.Persistence.Repositories;

internal class CompaniesRepository : Repository<CompanyEntity>, ICompaniesRepository
{
    #region Constructor

    public CompaniesRepository(MerchantPortalDbContext dbContext) : base(dbContext)
    { }

    #endregion
}