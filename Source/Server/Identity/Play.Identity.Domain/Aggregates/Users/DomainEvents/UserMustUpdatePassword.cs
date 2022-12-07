using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Identity.Domain.Aggregates;

public record UserMustUpdatePassword : BrokenRuleOrPolicyDomainEvent<User, SimpleStringId>
{
    #region Constructor

    public UserMustUpdatePassword(User user, IBusinessRule rule) : base(user, rule)
    { }

    #endregion
}