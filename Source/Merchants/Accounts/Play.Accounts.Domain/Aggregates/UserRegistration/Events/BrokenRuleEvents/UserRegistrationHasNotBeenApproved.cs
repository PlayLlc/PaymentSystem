using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UserRegistrationHasNotBeenApproved : BrokenBusinessRuleDomainEvent<UserRegistration, string>
{
    #region Constructor

    public UserRegistrationHasNotBeenApproved(UserRegistration userRegistration, IBusinessRule rule) : base(userRegistration, rule)
    { }

    #endregion
}