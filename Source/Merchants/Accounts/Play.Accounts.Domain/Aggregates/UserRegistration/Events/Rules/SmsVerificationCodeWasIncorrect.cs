using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record SmsVerificationCodeWasIncorrect : BrokenBusinessRuleDomainEvent<UserRegistration, string>
{
    #region Constructor

    public SmsVerificationCodeWasIncorrect(UserRegistration userRegistration, IBusinessRule rule) : base(userRegistration, rule)
    { }

    #endregion
}