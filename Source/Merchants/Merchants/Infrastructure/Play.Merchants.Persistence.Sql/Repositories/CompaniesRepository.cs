using Play.Merchants.Domain.Entities;
using Play.Merchants.Domain.Repositories;
using Play.Merchants.Persistence.Sql.Sql;
using Play.Persistence.Sql;

namespace Play.Merchants.Persistence.Sql.Repositories;

internal class CompaniesRepository : Repository<Company>, ICompaniesRepository
{
    #region Constructor

    public CompaniesRepository(MerchantPortalDbContext dbContext) : base(dbContext)
    { }

    #endregion
}