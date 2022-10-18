using Play.Accounts.Domain.Aggregates;
using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UserIsProhibitedFromRegistering : BusinessRuleViolationDomainEvent<UserRegistration, string>
{
    #region Constructor

    public UserIsProhibitedFromRegistering(UserRegistration userRegistration, IBusinessRule rule) : base(userRegistration, rule)
    { }

    #endregion
}