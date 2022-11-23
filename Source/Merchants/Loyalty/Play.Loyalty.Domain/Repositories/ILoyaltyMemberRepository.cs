using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;
using Play.Loyalty.Domain.Aggregates;

namespace Play.Loyalty.Domain.Repositories;

public interface ILoyaltyMemberRepository : IRepository<LoyaltyMember, SimpleStringId>
{
    #region Instance Members

    public Task RemoveAll(SimpleStringId merchantId);

    #endregion
}