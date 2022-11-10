using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record SmsVerificationCodeHasBeenSent : DomainEvent
{
    #region Instance Values

    public readonly UserRegistration UserRegistration;

    #endregion

    #region Constructor

    public SmsVerificationCodeHasBeenSent(UserRegistration userRegistration) : base(
        $"The SMS client has sent a confirmation code to the {nameof(UserRegistration)} with ID: [{userRegistration.GetId()}];")
    {
        UserRegistration = userRegistration;
    }

    #endregion
}