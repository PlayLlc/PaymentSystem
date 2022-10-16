using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.UserRegistration;

public record UserNameWasNotAValidEmailAddress : DomainEvent
{
    #region Static Metadata

    public static readonly DomainEventType DomainEventType = CreateEventTypeId(typeof(UserNameWasNotAValidEmailAddress));

    #endregion

    #region Instance Values

    public string Username;

    #endregion

    #region Constructor

    public UserNameWasNotAValidEmailAddress(string username) : base(DomainEventType)
    {
        Username = username;
    }

    #endregion
}