using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UserRegistrationHasExpired : BusinessRuleViolationDomainEvent<UserRegistration, string>
{
    #region Constructor

    public UserRegistrationHasExpired(UserRegistration merchantRegistration, IBusinessRule rule) : base(merchantRegistration, rule)
    { }

    #endregion
}