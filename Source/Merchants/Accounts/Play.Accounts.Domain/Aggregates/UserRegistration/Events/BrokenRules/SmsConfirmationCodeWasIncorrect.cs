using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record SmsConfirmationCodeWasIncorrect : BrokenBusinessRuleDomainEvent<UserRegistration, string>
{
    #region Constructor

    public SmsConfirmationCodeWasIncorrect(UserRegistration userRegistration, IBusinessRule rule) : base(userRegistration, rule)
    { }

    #endregion
}