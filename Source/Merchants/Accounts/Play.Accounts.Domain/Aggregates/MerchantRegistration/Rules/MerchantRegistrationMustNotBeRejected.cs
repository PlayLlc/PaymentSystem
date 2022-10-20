﻿using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Aggregates;
using Play.Globalization.Time;

namespace Play.Accounts.Domain.Aggregates;

internal class MerchantRegistrationMustNotBeRejected : BusinessRule<MerchantRegistration, string>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "Merchant cannot be created because the registration was rejected";

    #endregion

    #region Constructor

    public MerchantRegistrationMustNotBeRejected(MerchantRegistrationStatus status)
    {
        if (status == MerchantRegistrationStatuses.Rejected)
        {
            _IsValid = false;

            return;
        }

        _IsValid = true;
    }

    #endregion

    #region Instance Members

    public override MerchantRegistrationHasBeenRejected CreateBusinessRuleViolationDomainEvent(MerchantRegistration aggregate)
    {
        return new MerchantRegistrationHasBeenRejected(aggregate, this);
    }

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    #endregion
}