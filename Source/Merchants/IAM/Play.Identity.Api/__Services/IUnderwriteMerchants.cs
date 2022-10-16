using Play.Identity.Api.Identity.Entities;

namespace Play.Identity.Api.__Services;

public interface IUnderwriteMerchants
{
    #region Instance Members

    public bool IsMerchantProhibited();

    public bool IsIndustryProhibited();

    /// <summary>
    ///     Ensures that the user is not under sanctions, terrorism watch list, money laundering, etc..
    /// </summary>
    /// <returns></returns>
    public bool IsUserProhibited(UserIdentity user);

    #endregion
}