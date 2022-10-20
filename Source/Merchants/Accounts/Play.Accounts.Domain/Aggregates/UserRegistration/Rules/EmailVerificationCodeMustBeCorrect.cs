using Play.Domain.Aggregates;
using Play.Accounts.Domain.Entities;
using Play.Globalization.Time;

namespace Play.Accounts.Domain.Aggregates;

/// <summary>
///     The user's email must be verified during registration
/// </summary>
internal class EmailVerificationCodeMustBeCorrect : BusinessRule<UserRegistration, string>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "The user's email must be verified during registration";

    #endregion

    #region Constructor

    internal EmailVerificationCodeMustBeCorrect(ConfirmationCode emailConfirmationCode, uint confirmationCode)
    {
        _IsValid = emailConfirmationCode.Code == confirmationCode;
    }

    #endregion

    #region Instance Members

    public override EmailVerificationCodeWasIncorrect CreateBusinessRuleViolationDomainEvent(UserRegistration aggregate)
    {
        return new EmailVerificationCodeWasIncorrect(aggregate, this);
    }

    public override bool IsBroken()
    {
        return _IsValid;
    }

    #endregion
}