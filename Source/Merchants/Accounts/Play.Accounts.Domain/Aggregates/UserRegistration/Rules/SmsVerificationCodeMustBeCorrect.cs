using Play.Accounts.Domain.Entities;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

internal class SmsVerificationCodeMustBeCorrect : BusinessRule<UserRegistration, string>
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

    public override SmsVerificationCodeWasIncorrect CreateBusinessRuleViolationDomainEvent(UserRegistration merchant)
    {
        return new SmsVerificationCodeWasIncorrect(merchant, this);
    }

    public override bool IsBroken()
    {
        return _IsValid;
    }

    #endregion
}