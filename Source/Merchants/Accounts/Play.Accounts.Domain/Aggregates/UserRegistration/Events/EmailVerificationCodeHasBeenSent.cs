using Play.Accounts.Domain.Entities;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record EmailVerificationCodeHasBeenSent : DomainEvent
{
    #region Instance Values

    public readonly UserRegistration UserRegistration;

    #endregion

    #region Constructor

    public EmailVerificationCodeHasBeenSent(UserRegistration userRegistration) : base(
        $"A {nameof(ConfirmationCode)} has successfully been sent to the {nameof(UserRegistration)} with the ID: [{userRegistration.GetId()}];")
    {
        UserRegistration = userRegistration;
    }

    #endregion
}