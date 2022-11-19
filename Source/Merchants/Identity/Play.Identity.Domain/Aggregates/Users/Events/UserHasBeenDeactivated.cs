using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Identity.Domain.Aggregates.Events;

public record UserHasBeenDeactivated : BrokenRuleOrPolicyDomainEvent<User, SimpleStringId>
{
    #region Constructor

    public UserHasBeenDeactivated(User user, IBusinessRule rule) : base(user, rule)
    { }

    #endregion
}