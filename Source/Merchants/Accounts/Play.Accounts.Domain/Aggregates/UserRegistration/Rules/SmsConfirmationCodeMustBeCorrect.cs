using Play.Accounts.Domain.Entities;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

internal class SmsConfirmationCodeMustBeCorrect : BusinessRule<UserRegistration, string>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "The user's phone must be verified during registration";

    #endregion

    #region Constructor

    internal SmsConfirmationCodeMustBeCorrect(ConfirmationCode emailConfirmationCode, uint confirmationCode)
    {
        _IsValid = emailConfirmationCode.Code == confirmationCode;
    }

    #endregion

    #region Instance Members

    public override SmsConfirmationCodeWasIncorrect CreateBusinessRuleViolationDomainEvent(UserRegistration aggregate)
    {
        return new SmsConfirmationCodeWasIncorrect(aggregate, this);
    }

    public override bool IsBroken()
    {
        return _IsValid;
    }

    #endregion
}