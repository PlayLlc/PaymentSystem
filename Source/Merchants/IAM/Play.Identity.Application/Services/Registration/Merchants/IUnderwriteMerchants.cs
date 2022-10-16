using Play.Identity.Domain;

namespace Play.Identity.Application.Services.Registration.Merchants;

public interface IUnderwriteMerchants
{
    #region Instance Members

    public bool IsMerchantProhibited(Name name, Address address);

    public bool IsIndustryProhibited(MerchantCategoryCodes categoryCodes);

    /// <summary>
    ///     Ensures that the user is not under sanctions, terrorism watch list, money laundering, etc..
    /// </summary>
    /// <returns></returns>
    public bool IsUserProhibited(Address address, ContactInfo contactInfo);

    #endregion
}