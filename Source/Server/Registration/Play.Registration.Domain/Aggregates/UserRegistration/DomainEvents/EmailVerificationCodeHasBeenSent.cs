using Play.Domain.Events;
using Play.Registration.Domain.Entities;

namespace Play.Registration.Domain.Aggregates.UserRegistration.DomainEvents;

public record EmailVerificationCodeHasBeenSent : DomainEvent
{
    #region Instance Values

    public readonly UserRegistration UserRegistration;

    #endregion

    #region Constructor

    public EmailVerificationCodeHasBeenSent(UserRegistration userRegistration) : base(
        $"A {nameof(EmailConfirmationCode)} has successfully been sent to the {nameof(UserRegistration)} with the ID: [{userRegistration.GetId()}];")
    {
        UserRegistration = userRegistration;
    }

    #endregion
}