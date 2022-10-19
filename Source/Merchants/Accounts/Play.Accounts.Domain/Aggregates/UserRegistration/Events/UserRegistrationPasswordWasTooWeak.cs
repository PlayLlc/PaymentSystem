using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UserRegistrationPasswordWasTooWeak : BusinessRuleViolationDomainEvent<UserRegistration, string>
{
    #region Constructor

    public UserRegistrationPasswordWasTooWeak(UserRegistration userRegistration, IBusinessRule rule) : base(userRegistration, rule)
    { }

    #endregion
}