﻿using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record SmsVerificationCodeFailedToSend : DomainEvent
{
    #region Instance Values

    public readonly UserRegistration UserRegistration;

    #endregion

    #region Constructor

    public SmsVerificationCodeFailedToSend(UserRegistration userRegistration) : base(
        $"The SMS client failed to send a confirmation code for {nameof(UserRegistration)} with ID: [{userRegistration.GetId()}];")
    {
        UserRegistration = userRegistration;
    }

    #endregion
}