using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.Services;

namespace Play.Loyalty.Domain.Repositories;

public interface ILoyaltyMemberRepository : IRepository<Member, SimpleStringId>, IEnsureRewardsNumbersAreUnique
{
    #region Instance Members

    public Task RemoveAll(SimpleStringId merchantId);

    #endregion
}