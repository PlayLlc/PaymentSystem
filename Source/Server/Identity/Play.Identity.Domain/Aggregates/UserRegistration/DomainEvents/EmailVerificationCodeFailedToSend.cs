using Play.Domain.Events;

namespace Play.Identity.Domain.Aggregates;

public record EmailVerificationCodeFailedToSend : DomainEvent
{
    #region Instance Values

    public readonly UserRegistration UserRegistration;

    #endregion

    #region Constructor

    public EmailVerificationCodeFailedToSend(UserRegistration userRegistration) : base(
        $"The email client failed to send a confirmation code for {nameof(UserRegistration)} with the ID: [{userRegistration.GetId()}];")
    {
        UserRegistration = userRegistration;
    }

    #endregion
}