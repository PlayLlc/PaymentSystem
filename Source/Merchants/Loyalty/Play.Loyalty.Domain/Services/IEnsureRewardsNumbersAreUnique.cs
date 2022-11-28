using Play.Domain.Common.ValueObjects;

namespace Play.Loyalty.Domain.Services;

public interface IEnsureRewardsNumbersAreUnique
{
    #region Instance Members

    public bool IsRewardsNumberUnique(SimpleStringId merchantId, string rewardsNumber);

    #endregion
}