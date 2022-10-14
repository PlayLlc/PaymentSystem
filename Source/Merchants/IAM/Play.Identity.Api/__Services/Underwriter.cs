using Play.Identity.Api.Identity.Entities;

namespace Play.Identity.Api.Services;

public class Underwriter : IUnderwriteMerchants
{
    #region Instance Members

    public bool IsMerchantProhibited()
    {
        return false;
    }

    public bool IsIndustryProhibited()
    {
        return false;
    }

    public bool IsUserProhibited(UserIdentity user)
    {
        return false;
    }

    #endregion
}