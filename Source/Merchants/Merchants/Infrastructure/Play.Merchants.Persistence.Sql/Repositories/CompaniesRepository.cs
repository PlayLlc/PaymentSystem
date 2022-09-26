using Play.MerchantPortal.Domain.Entities;
using Play.MerchantPortal.Domain.Persistence;
using Play.MerchantPortal.Infrastructure.Persistence.Sql;

namespace Play.MerchantPortal.Infrastructure.Persistence.Repositories;

internal class CompaniesRepository : Repository<Company>, ICompaniesRepository
{
    #region Constructor

    public CompaniesRepository(MerchantPortalDbContext dbContext) : base(dbContext)
    { }

    #endregion
}