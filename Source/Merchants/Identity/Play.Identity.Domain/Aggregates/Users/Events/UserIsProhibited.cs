using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Identity.Domain.Aggregates.Events;

public record UserIsProhibited : BrokenRuleOrPolicyDomainEvent<User, SimpleStringId>
{
    #region Instance Values

    public readonly User User;

    #endregion

    #region Constructor

    public UserIsProhibited(User user, IBusinessRule rule) : base(user, rule)
    {
        User = user;
    }

    #endregion
}