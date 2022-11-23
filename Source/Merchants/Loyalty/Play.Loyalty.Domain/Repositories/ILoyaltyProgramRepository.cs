using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;
using Play.Loyalty.Domain.Aggregates;

namespace Play.Loyalty.Domain.Repositories;

public interface ILoyaltyProgramRepository : IRepository<LoyaltyProgram, SimpleStringId>
{
    #region Instance Members

    public Task<LoyaltyProgram> GetByMerchantIdAsync(SimpleStringId merchantId);

    #endregion
}

public interface ILoyaltyMemberRepository : IRepository<LoyaltyMember, SimpleStringId>
{ }