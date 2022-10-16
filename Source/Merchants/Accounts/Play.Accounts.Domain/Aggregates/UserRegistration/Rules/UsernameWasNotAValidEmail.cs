using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.UserRegistration;

public record UsernameWasNotAValidEmail : DomainEvent
{
    #region Static Metadata

    public static readonly DomainEventType DomainEventType = CreateEventTypeId(typeof(UsernameWasNotAValidEmail));

    #endregion

    #region Instance Values

    public readonly string Username;

    #endregion

    #region Constructor

    public UsernameWasNotAValidEmail(string username) : base(DomainEventType)
    { }

    #endregion
}