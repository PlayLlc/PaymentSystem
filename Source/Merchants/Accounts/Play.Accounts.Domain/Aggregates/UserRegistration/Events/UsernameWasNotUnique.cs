﻿using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UsernameWasNotUnique : BusinessRuleViolationDomainEvent<UserRegistration, string>
{
    #region Constructor

    public UsernameWasNotUnique(UserRegistration userRegistration, IBusinessRule rule) : base(userRegistration, rule)
    { }

    #endregion
}