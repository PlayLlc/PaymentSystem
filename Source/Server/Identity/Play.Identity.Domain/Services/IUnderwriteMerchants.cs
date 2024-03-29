﻿using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Identity.Domain.Entities;
using Play.Identity.Domain.ValueObjects;

namespace Play.Identity.Domain.Services;

public interface IUnderwriteMerchants
{
    #region Instance Members

    public Task<bool> IsMerchantProhibited(Name name, Address address);

    public Task<bool> IsIndustryProhibited(MerchantCategoryCode categoryCodes);

    /// <summary>
    ///     Ensures that the user is not under sanctions, terrorism watch list, money laundering, etc..
    /// </summary>
    /// <returns></returns>
    public Task<bool> IsUserProhibited(PersonalDetail personalDetail, Address address, Contact contact);

    #endregion
}