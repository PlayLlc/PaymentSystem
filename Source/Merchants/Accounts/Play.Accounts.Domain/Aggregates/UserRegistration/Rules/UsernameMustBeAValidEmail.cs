﻿using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

internal class UsernameMustBeAValidEmail : BusinessRule<UserRegistration, string>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "Username must be a valid email address";

    #endregion

    #region Constructor

    internal UsernameMustBeAValidEmail(string username)
    {
        _IsValid = Email.IsValid(username);
    }

    #endregion

    #region Instance Members

    public override UsernameWasNotAValidEmail CreateBusinessRuleViolationDomainEvent(UserRegistration aggregate)
    {
        return new UsernameWasNotAValidEmail(aggregate, this);
    }

    public override bool IsBroken()
    {
        return _IsValid;
    }

    #endregion
}