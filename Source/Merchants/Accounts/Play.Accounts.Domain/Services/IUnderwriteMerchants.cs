using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.ValueObjects;

namespace Play.Accounts.Domain.Services;

public interface IUnderwriteMerchants
{
    #region Instance Members

    public bool IsMerchantProhibited(Name name, Address address);

    public bool IsIndustryProhibited(MerchantCategoryCode categoryCodes);

    /// <summary>
    ///     Ensures that the user is not under sanctions, terrorism watch list, money laundering, etc..
    /// </summary>
    /// <returns></returns>
    public bool IsUserProhibited(PersonalDetail personalDetail, Address address, Contact contact);

    #endregion
}