using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UserHasBeenDeactivated : BrokenBusinessRuleDomainEvent<User, string>
{
    #region Constructor

    public UserHasBeenDeactivated(User user, IBusinessRule rule) : base(user, rule)
    { }

    #endregion
}