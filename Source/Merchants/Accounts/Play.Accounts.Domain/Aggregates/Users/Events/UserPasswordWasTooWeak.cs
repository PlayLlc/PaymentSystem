using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UserPasswordWasTooWeak : BusinessRuleViolationDomainEvent<User, string>
{
    #region Constructor

    public UserPasswordWasTooWeak(User user, IBusinessRule rule) : base(user, rule)
    { }

    #endregion
}