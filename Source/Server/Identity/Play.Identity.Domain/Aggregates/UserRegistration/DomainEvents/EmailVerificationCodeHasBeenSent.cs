using Play.Domain.Events;
using Play.Identity.Domain.Entities;

namespace Play.Identity.Domain.Aggregates;

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