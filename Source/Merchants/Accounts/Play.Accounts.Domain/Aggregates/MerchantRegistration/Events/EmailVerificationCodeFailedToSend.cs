using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record EmailVerificationCodeFailedToSend : DomainEvent
{
    #region Instance Values

    public readonly string Id;

    #endregion

    #region Constructor

    public EmailVerificationCodeFailedToSend(string id) : base(
        $"The email client failed to send a confirmation code for {nameof(UserRegistration)} with {nameof(Id)}: [{id}];")
    {
        Id = id;
    }

    #endregion
}