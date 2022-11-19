﻿using Play.Domain.Aggregates;
using Play.Identity.Domain.Aggregates.Events;
using Play.Identity.Domain.Services;

namespace Play.Identity.Domain.Aggregates.Rules;

internal class LastFourUserPasswordsMustBeUnique : BusinessRule<User, string>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "The user's last four passwords must be unique";

    #endregion

    #region Constructor

    /// <exception cref="AggregateException"></exception>
    internal LastFourUserPasswordsMustBeUnique(IEnsureUniquePasswordHistory uniquePasswordChecker, string userId, string hashedPassword)
    {
        Task<bool> a = uniquePasswordChecker.AreLastFourPasswordsUnique(userId, hashedPassword);
        Task.WhenAll(a);
        _IsValid = a.Result;
    }

    #endregion

    #region Instance Members

    public override UserPasswordWasTooWeak CreateBusinessRuleViolationDomainEvent(User user)
    {
        return new UserPasswordWasTooWeak(user, this);
    }

    public override bool IsBroken()
    {
        return _IsValid;
    }

    #endregion
}