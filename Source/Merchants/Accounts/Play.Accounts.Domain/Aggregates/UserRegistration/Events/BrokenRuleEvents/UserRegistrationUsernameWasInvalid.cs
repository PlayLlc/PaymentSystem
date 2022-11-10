using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UserRegistrationUsernameWasInvalid : BrokenBusinessRuleDomainEvent<UserRegistration, string>
{
    #region Constructor

    public UserRegistrationUsernameWasInvalid(UserRegistration userRegistration, IBusinessRule rule) : base(userRegistration, rule)
    { }

    #endregion
}