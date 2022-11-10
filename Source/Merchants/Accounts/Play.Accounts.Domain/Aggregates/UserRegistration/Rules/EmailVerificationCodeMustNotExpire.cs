using Play.Domain.Aggregates;
using Play.Accounts.Domain.Entities;

namespace Play.Accounts.Domain.Aggregates;

internal class EmailVerificationCodeMustNotExpire : BusinessRule<UserRegistration, string>
{
    #region Static Metadata

    private static readonly TimeSpan _ValidityPeriod = new(0, 4, 0, 0);

    #endregion

    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "The user's email must be verified during registration";

    #endregion

    #region Constructor

    internal EmailVerificationCodeMustNotExpire(ConfirmationCode emailConfirmationCode)
    {
        _IsValid = emailConfirmationCode.IsExpired(_ValidityPeriod);
    }

    #endregion

    #region Instance Members

    public override EmailVerificationCodeHasExpired CreateBusinessRuleViolationDomainEvent(UserRegistration merchant)
    {
        return new EmailVerificationCodeHasExpired(merchant, this);
    }

    public override bool IsBroken()
    {
        return _IsValid;
    }

    #endregion
}