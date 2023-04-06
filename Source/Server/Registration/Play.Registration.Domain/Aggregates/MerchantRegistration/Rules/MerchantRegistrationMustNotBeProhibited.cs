﻿using Play.Domain.Aggregates;
using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Registration.Domain.Aggregates.MerchantRegistration.DomainEvents.Rules;
using Play.Registration.Domain.Services;

namespace Play.Registration.Domain.Aggregates.MerchantRegistration.Rules;

internal class MerchantRegistrationMustNotBeProhibited : BusinessRule<MerchantRegistration>
{
    #region Instance Values

    private readonly bool _IsProhibited;

    public override string Message => "Merchant is prohibited from conducting commerce by the government";

    #endregion

    #region Constructor

    /// <exception cref="AggregateException"></exception>
    public MerchantRegistrationMustNotBeProhibited(IUnderwriteMerchants merchantUnderwriter, Name companyName, Address address)
    {
        _IsProhibited = IsProhibited(merchantUnderwriter, companyName, address);
    }

    #endregion

    #region Instance Members

    /// <exception cref="AggregateException"></exception>
    public bool IsProhibited(IUnderwriteMerchants merchantUnderwriter, Name companyName, Address address)
    {
        Task<bool> merchantProhibitedTask = merchantUnderwriter.IsMerchantProhibited(companyName, address);
        Task.WhenAll(merchantProhibitedTask);

        // ReSharper disable once RedundantSuppressNullableWarningExpression
        return merchantProhibitedTask!.Result;
    }

    public override bool IsBroken() => _IsProhibited;

    public override MerchantRegistrationHasBeenRejected CreateBusinessRuleViolationDomainEvent(MerchantRegistration merchant) => new(merchant, this);

    #endregion
}