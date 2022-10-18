using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record EmailConfirmationCodeWasIncorrect : BusinessRuleViolationDomainEvent<UserRegistration, string>
{
    #region Constructor

    public EmailConfirmationCodeWasIncorrect(UserRegistration userRegistration, IBusinessRule rule) : base(userRegistration, rule)
    { }

    #endregion
}