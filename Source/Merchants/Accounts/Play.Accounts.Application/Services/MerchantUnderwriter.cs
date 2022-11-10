using Play.Accounts.Domain.Services;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.ValueObjects;

namespace Play.Accounts.Application.Services;

public class MerchantUnderwriter : IUnderwriteMerchants
{
    #region Instance Members

    public Task<bool> IsMerchantProhibited(Name name, Address address)
    {
        return Task.FromResult(false);
    }

    public Task<bool> IsIndustryProhibited(MerchantCategoryCode categoryCodes)
    {
        return Task.FromResult(false);
    }

    public Task<bool> IsUserProhibited(PersonalDetail personalDetail, Address address, Contact contact)
    {
        return Task.FromResult(false);
    }

    #endregion
}