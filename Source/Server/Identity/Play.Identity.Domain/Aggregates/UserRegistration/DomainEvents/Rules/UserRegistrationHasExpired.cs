using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Identity.Domain.Aggregates;

public record UserRegistrationHasExpired : BrokenRuleOrPolicyDomainEvent<UserRegistration, SimpleStringId>
{
    #region Constructor

    public UserRegistrationHasExpired(UserRegistration merchantRegistration, IBusinessRule rule) : base(merchantRegistration, rule)
    { }

    #endregion
}