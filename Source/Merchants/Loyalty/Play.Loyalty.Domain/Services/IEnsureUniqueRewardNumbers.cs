namespace Play.Identity.Domain.Serviceddds;

public interface IEnsureUniqueRewardNumbers
{
    #region Instance Members

    public Task<bool> IsUniqueAsync(string merchantId, string rewardNumber);
    public bool IsUnique(string merchantId, string rewardNumber);

    #endregion
}