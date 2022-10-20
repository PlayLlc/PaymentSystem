﻿using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

internal class UserRegistrationMustNotBeRejected : BusinessRule<UserRegistration, string>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "User cannot complete registration because the user is prohibited";

    #endregion

    #region Constructor

    public UserRegistrationMustNotBeRejected(UserRegistrationStatus status)
    {
        if (status == UserRegistrationStatuses.Rejected)
        {
            _IsValid = false;

            return;
        }

        _IsValid = true;
    }

    #endregion

    #region Instance Members

    public override UserRegistrationHasBeenRejected CreateBusinessRuleViolationDomainEvent(UserRegistration aggregate)
    {
        return new UserRegistrationHasBeenRejected(aggregate, this);
    }

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    #endregion
}