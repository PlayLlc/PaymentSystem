using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;
using Play.Loyalty.Domain.Aggregates;

namespace Play.Loyalty.Domain.Repositories;

public interface ILoyaltyProgramRepository : IRepository<Programs, SimpleStringId>
{
    #region Instance Members

    public Task<Programs?> GetByMerchantIdAsync(SimpleStringId merchantId);

    #endregion
}