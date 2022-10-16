using Microsoft.EntityFrameworkCore;

using Play.Accounts.Domain.Aggregates.MerchantRegistration;
using Play.Persistence.Sql;

namespace Play.Accounts.Persistence.Sql.Repositories;

public class MerchantRegistrationRepository : Repository<MerchantRegistration, string>
{
    #region Constructor

    public MerchantRegistrationRepository(DbContext dbContext) : base(dbContext)
    { }

    #endregion
}