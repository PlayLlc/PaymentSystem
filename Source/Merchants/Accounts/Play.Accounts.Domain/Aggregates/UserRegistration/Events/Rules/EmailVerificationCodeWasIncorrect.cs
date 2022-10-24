using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record EmailVerificationCodeWasIncorrect : BrokenBusinessRuleDomainEvent<UserRegistration, string>
{
    #region Constructor

    public EmailVerificationCodeWasIncorrect(UserRegistration userRegistration, IBusinessRule rule) : base(userRegistration, rule)
    { }

    #endregion
}