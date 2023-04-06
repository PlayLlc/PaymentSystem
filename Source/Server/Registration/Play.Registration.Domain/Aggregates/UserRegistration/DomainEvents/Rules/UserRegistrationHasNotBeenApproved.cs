using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Registration.Domain.Aggregates.UserRegistration.DomainEvents.Rules;

public record UserRegistrationHasNotBeenApproved : BrokenRuleOrPolicyDomainEvent<UserRegistration, SimpleStringId>
{
    #region Constructor

    public UserRegistrationHasNotBeenApproved(UserRegistration userRegistration, IBusinessRule rule) : base(userRegistration, rule)
    { }

    #endregion
}