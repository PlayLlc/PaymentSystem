using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record SmsVerificationCodeFailedToSend : DomainEvent
{
    #region Instance Values

    public readonly string Id;

    #endregion

    #region Constructor

    public SmsVerificationCodeFailedToSend(string id) : base(
        $"The sms client failed to send a confirmation code for {nameof(UserRegistration)} with {nameof(Id)}: [{id}];")
    {
        Id = id;
    }

    #endregion
}