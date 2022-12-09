﻿using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Globalization.Time;
using Play.Identity.Domain.Enums;
using Play.Identity.Domain.ValueObjects;

namespace Play.Identity.Domain.Aggregates;

internal class UserRegistrationMustNotExpire : BusinessRule<UserRegistration, SimpleStringId>
{
    #region Instance Values

    private readonly TimeSpan _ValidityPeriod = new(7, 0, 0, 0);
    private readonly bool _IsValid;

    public override string Message => "User cannot be created when registration has expired";

    #endregion

    #region Constructor

    public UserRegistrationMustNotExpire(UserRegistrationStatus status, DateTimeUtc registeredDate)
    {
        if (status == UserRegistrationStatuses.Expired)
        {
            _IsValid = false;

            return;
        }

        if ((DateTimeUtc.Now - registeredDate) > _ValidityPeriod)
        {
            _IsValid = false;

            return;
        }

        _IsValid = true;
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override UserRegistrationHasExpired CreateBusinessRuleViolationDomainEvent(UserRegistration merchant) => new(merchant, this);

    #endregion
}