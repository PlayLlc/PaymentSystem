﻿using Play.Domain;
using Play.Merchants.Onboarding.Domain.Services;
using Play.Merchants.Onboarding.Domain.ValueObjects;

namespace Play.Merchants.Onboarding.Domain.Aggregates;

internal class UserEmailMustBeUnique : IBusinessRule
{
    #region Instance Values

    private readonly IEnsureUniqueEmails _UniqueEmailChecker;
    private readonly Email _Email;

    public string Message => "User Login must be unique";

    #endregion

    #region Constructor

    internal UserEmailMustBeUnique(IEnsureUniqueEmails uniqueEmailChecker, Email email)
    {
        _UniqueEmailChecker = uniqueEmailChecker;
        _Email = email;
    }

    #endregion

    #region Instance Members

    public bool IsBroken()
    {
        if (!_UniqueEmailChecker.IsUnique(_Email))
            return true;

        return false;
    }

    #endregion
}