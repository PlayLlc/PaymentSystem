﻿using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Services;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

internal class MerchantMustNotBeProhibited : BusinessRule<Merchant, string>
{
    #region Instance Values

    private readonly bool _IsProhibited;

    public override string Message => "Merchant is prohibited from conducting commerce by the government";

    #endregion

    #region Constructor

    /// <exception cref="AggregateException"></exception>
    public MerchantMustNotBeProhibited(IUnderwriteMerchants merchantUnderwriter, Name companyName, Address address)
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

    public override bool IsBroken()
    {
        return _IsProhibited;
    }

    public override MerchantIsProhibited CreateBusinessRuleViolationDomainEvent(Merchant merchant)
    {
        return new MerchantIsProhibited(merchant, this);
    }

    #endregion
}