using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Identity.Domain.Entities;
using Play.Identity.Domain.Services;
using Play.Identity.Domain.ValueObjects;

namespace Play.Identity.Application.Services;

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