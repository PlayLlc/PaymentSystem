﻿using Play.Accounts.Domain.Aggregates.MerchantRegistration.Events;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Services;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates.MerchantRegistration;

internal class MerchantMustNotBeProhibited : BusinessRule<MerchantRegistration, string>
{
    #region Instance Values

    private readonly bool _IsProhibited;

    public override string Message => "Merchant is prohibited from conducting commerce by the government";

    #endregion

    #region Constructor

    public MerchantMustNotBeProhibited(IUnderwriteMerchants merchantUnderwriter, Name companyName, Address address)
    {
        _IsProhibited = merchantUnderwriter.IsMerchantProhibited(companyName, address);
    }

    #endregion

    #region Instance Members

    public override bool IsBroken()
    {
        return _IsProhibited;
    }

    public override MerchantRejectedBecauseItIsProhibited CreateBusinessRuleViolationDomainEvent(MerchantRegistration merchantRegistration)
    {
        return new MerchantRejectedBecauseItIsProhibited(merchantRegistration, this);
    }

    #endregion
}