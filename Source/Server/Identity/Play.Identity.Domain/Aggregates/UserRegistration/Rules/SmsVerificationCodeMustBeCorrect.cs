using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Identity.Domain.Entities;

namespace Play.Identity.Domain.Aggregates;

internal class SmsVerificationCodeMustBeCorrect : BusinessRule<UserRegistration, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "The user's phone must be verified during registration";

    #endregion

    #region Constructor

    internal SmsVerificationCodeMustBeCorrect(ConfirmationCode emailConfirmationCode, uint confirmationCode)
    {
        _IsValid = emailConfirmationCode.Code == confirmationCode;
    }

    #endregion

    #region Instance Members

    public override SmsVerificationCodeWasIncorrect CreateBusinessRuleViolationDomainEvent(UserRegistration merchant) => new(merchant, this);

    public override bool IsBroken() => _IsValid;

    #endregion
}