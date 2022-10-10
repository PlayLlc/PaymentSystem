﻿using Play.Accounts.Domain.Enums;
using Play.Domain;

namespace Play.Accounts.Domain.Aggregates.UserRegistration;

internal class UserCannotBeCreatedWhenRegistrationIsNotConfirmed : IBusinessRule
{
    #region Instance Values

    private readonly RegistrationStatuses _RegistrationStatus;

    public string Message => "User cannot be created when registration is not confirmed";

    #endregion

    #region Constructor

    internal UserCannotBeCreatedWhenRegistrationIsNotConfirmed(RegistrationStatuses registrationStatus)
    {
        _RegistrationStatus = registrationStatus;
    }

    #endregion

    #region Instance Members

    public bool IsBroken()
    {
        return _RegistrationStatus != RegistrationStatuses.Confirmed;
    }

    #endregion
}