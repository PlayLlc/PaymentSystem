using Play.Domain.Aggregates;
using Play.Accounts.Domain.Entities;
using Play.Globalization.Time;

namespace Play.Accounts.Domain.Aggregates;

/// <summary>
///     The user's email must be verified during registration
/// </summary>
internal class EmailConfirmationCodeMustBeVerified : BusinessRule<UserRegistration, string>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "The user's email must be verified during registration";

    #endregion

    #region Constructor

    internal EmailConfirmationCodeMustBeVerified(ConfirmationCode emailConfirmationCode, uint confirmationCode)
    {
        _IsValid = emailConfirmationCode.Code == confirmationCode;
    }

    #endregion

    #region Instance Members

    public override EmailConfirmationCodeWasIncorrect CreateBusinessRuleViolationDomainEvent(UserRegistration aggregate)
    {
        return new EmailConfirmationCodeWasIncorrect(aggregate, this);
    }

    public override bool IsBroken()
    {
        return _IsValid;
    }

    #endregion
}