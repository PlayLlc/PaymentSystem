using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record SmsConfirmationCodeHasExpired : BrokenBusinessRuleDomainEvent<UserRegistration, string>
{
    #region Constructor

    public SmsConfirmationCodeHasExpired(UserRegistration userRegistration, IBusinessRule rule) : base(userRegistration, rule)
    { }

    #endregion
}