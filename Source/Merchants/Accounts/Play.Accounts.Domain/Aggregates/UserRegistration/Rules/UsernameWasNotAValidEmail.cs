using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.UserRegistration.Rules;

public record UsernameWasNotAValidEmail : DomainEvent
{
    #region Static Metadata

    public static readonly DomainEventTypeId DomainEventTypeId = CreateEventTypeId(typeof(UsernameWasNotAValidEmail));

    #endregion

    #region Instance Values

    public readonly string Username;

    #endregion

    #region Constructor

    public UsernameWasNotAValidEmail(string username) : base(DomainEventTypeId)
    { }

    #endregion
}