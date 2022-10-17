using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UsernameWasNotAValidEmail : BusinessRuleViolationDomainEvent<UserRegistration, string>
{
    #region Constructor

    public UsernameWasNotAValidEmail(UserRegistration userRegistration, IBusinessRule rule) : base(userRegistration, rule)
    { }

    #endregion
}