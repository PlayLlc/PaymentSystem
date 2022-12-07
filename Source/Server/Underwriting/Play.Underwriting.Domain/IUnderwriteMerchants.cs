using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Underwriting.Domain.ValueObjects;

namespace Play.Underwriting.Domain;

public interface IUnderwriteMerchants
{
    #region Instance Members

    public Task<bool> IsMerchantProhibited(Name name, Address address);

    public Task<bool> IsIndustryProhibited(MerchantCategoryCode categoryCodes);

    /// <summary>
    ///     Ensures that the user is not under sanctions, terrorism watch list, money laundering, etc..
    /// </summary>
    /// <returns></returns>
    public Task<bool> IsUserProhibited(Address address, Contact contact);

    #endregion
}