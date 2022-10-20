using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UserRegistrationIsProhibited : BrokenBusinessRuleDomainEvent<UserRegistration, string>
{
    #region Constructor

    public UserRegistrationIsProhibited(UserRegistration userRegistration, IBusinessRule rule) : base(userRegistration, rule)
    { }

    #endregion
}