using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Identity.Domain.Aggregates;

public record EmailVerificationCodeWasIncorrect : BrokenRuleOrPolicyDomainEvent<UserRegistration, SimpleStringId>
{
    #region Constructor

    public EmailVerificationCodeWasIncorrect(UserRegistration userRegistration, IBusinessRule rule) : base(userRegistration, rule)
    { }

    #endregion
}