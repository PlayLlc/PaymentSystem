﻿using Play.Accounts.Domain.Services;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

internal class UserRegistrationUsernameMustBeUnique : BusinessRule<UserRegistration, string>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "Username must be unique";

    #endregion

    #region Constructor

    /// <exception cref="AggregateException"></exception>
    internal UserRegistrationUsernameMustBeUnique(IEnsureUniqueEmails uniqueEmailChecker, string email)
    {
        Task<bool> taskResult = uniqueEmailChecker.IsUnique(email);
        Task.WhenAll(taskResult);
        _IsValid = taskResult.Result;
    }

    #endregion

    #region Instance Members

    public override UserRegistrationUsernameWasInvalid CreateBusinessRuleViolationDomainEvent(UserRegistration merchant)
    {
        return new UserRegistrationUsernameWasInvalid(merchant, this);
    }

    public override bool IsBroken()
    {
        return _IsValid;
    }

    #endregion
}