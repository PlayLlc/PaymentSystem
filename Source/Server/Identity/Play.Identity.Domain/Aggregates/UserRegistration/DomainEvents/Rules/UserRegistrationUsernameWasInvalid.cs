using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Identity.Domain.Aggregates;

public record UserRegistrationUsernameWasInvalid : BrokenRuleOrPolicyDomainEvent<UserRegistration, SimpleStringId>
{
    #region Constructor

    public UserRegistrationUsernameWasInvalid(UserRegistration userRegistration, IBusinessRule rule) : base(userRegistration, rule)
    { }

    #endregion
}