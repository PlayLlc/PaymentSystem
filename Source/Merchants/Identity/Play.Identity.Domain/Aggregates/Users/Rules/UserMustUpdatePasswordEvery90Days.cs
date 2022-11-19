﻿using Play.Domain.Aggregates;
using Play.Identity.Domain.Aggregates.Events;
using Play.Identity.Domain.Entities;

namespace Play.Identity.Domain.Aggregates.Rules;

internal class UserMustUpdatePasswordEvery90Days : BusinessRule<User, string>
{
    #region Instance Values

    private readonly bool _IsValid;
    private readonly TimeSpan _ValidityPeriod = new(90, 0, 0, 0);
    public override string Message => "The login attempt has failed because the user's password has expired";

    #endregion

    #region Constructor

    internal UserMustUpdatePasswordEvery90Days(Password password)
    {
        _IsValid = !password.IsExpired(_ValidityPeriod);
    }

    #endregion

    #region Instance Members

    public override UserMustUpdatePassword CreateBusinessRuleViolationDomainEvent(User merchant)
    {
        return new UserMustUpdatePassword(merchant, this);
    }

    public override bool IsBroken()
    {
        return _IsValid;
    }

    #endregion
}