using Microsoft.EntityFrameworkCore;

using Play.Accounts.Domain.Aggregates.Merchants;
using Play.Persistence.Sql;

namespace Play.Accounts.Persistence.Sql.Repositories;

public class MerchantRepository : Repository<Merchant, string>
{
    #region Constructor

    public MerchantRepository(DbContext dbContext) : base(dbContext)
    { }

    #endregion
}