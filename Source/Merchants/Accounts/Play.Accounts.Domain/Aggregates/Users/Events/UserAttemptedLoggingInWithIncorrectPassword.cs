using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UserAttemptedLoggingInWithIncorrectPassword : BrokenBusinessRuleDomainEvent<User, string>
{
    #region Constructor

    public UserAttemptedLoggingInWithIncorrectPassword(User user, IBusinessRule rule) : base(user, rule)
    { }

    #endregion
}