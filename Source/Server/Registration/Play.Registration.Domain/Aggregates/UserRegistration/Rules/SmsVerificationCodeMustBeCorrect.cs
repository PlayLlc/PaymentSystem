using Play.Domain.Aggregates;
using Play.Registration.Domain.Aggregates.UserRegistration.DomainEvents.Rules;
using Play.Registration.Domain.Entities;

namespace Play.Registration.Domain.Aggregates.UserRegistration.Rules;

internal class SmsVerificationCodeMustBeCorrect : BusinessRule<UserRegistration>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "The user's phone must be verified during registration";

    #endregion

    #region Constructor

    internal SmsVerificationCodeMustBeCorrect(SmsConfirmationCode emailConfirmationCode, uint confirmationCode)
    {
        _IsValid = emailConfirmationCode.Code == confirmationCode;
    }

    #endregion

    #region Instance Members

    public override SmsVerificationCodeWasIncorrect CreateBusinessRuleViolationDomainEvent(UserRegistration merchant) => new(merchant, this);

    public override bool IsBroken() => !_IsValid;

    #endregion
}