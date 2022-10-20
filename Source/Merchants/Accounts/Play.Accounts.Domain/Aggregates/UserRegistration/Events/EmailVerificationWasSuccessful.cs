using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record EmailVerificationWasSuccessful : DomainEvent
{
    #region Instance Values

    public readonly UserRegistration UserRegistration;

    #endregion

    #region Constructor

    public EmailVerificationWasSuccessful(UserRegistration userRegistration) : base(
        $"The {nameof(UserRegistration)} with the ID: [{userRegistration.GetId()}] successfully verified their {nameof(Email)};")
    {
        UserRegistration = userRegistration;
    }

    #endregion
}