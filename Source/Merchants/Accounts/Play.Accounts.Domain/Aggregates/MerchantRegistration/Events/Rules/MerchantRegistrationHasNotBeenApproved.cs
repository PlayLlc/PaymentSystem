﻿using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record MerchantRegistrationHasNotBeenApproved : BrokenBusinessRuleDomainEvent<MerchantRegistration, string>
{
    #region Constructor

    public MerchantRegistrationHasNotBeenApproved(MerchantRegistration merchantRegistration, IBusinessRule rule) : base(merchantRegistration, rule)
    { }

    #endregion
}