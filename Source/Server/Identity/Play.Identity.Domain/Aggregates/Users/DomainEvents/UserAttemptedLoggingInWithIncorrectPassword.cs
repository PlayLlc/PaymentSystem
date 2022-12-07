using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Identity.Domain.Aggregates;

public record UserAttemptedLoggingInWithIncorrectPassword : BrokenRuleOrPolicyDomainEvent<User, SimpleStringId>
{
    #region Constructor

    public UserAttemptedLoggingInWithIncorrectPassword(User user, IBusinessRule rule) : base(user, rule)
    { }

    #endregion
}