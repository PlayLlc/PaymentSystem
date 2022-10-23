using Play.Accounts.Domain.Entities;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

internal class SmsVerificationCodeMustNotBeExpired : BusinessRule<UserRegistration, string>
{
    #region Static Metadata

    private static readonly TimeSpan _ValidityPeriod = new(0, 4, 0, 0);

    #endregion

    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "The user's email must be verified during registration";

    #endregion

    #region Constructor

    internal SmsVerificationCodeMustNotBeExpired(ConfirmationCode emailConfirmationCode)
    {
        _IsValid = emailConfirmationCode.IsExpired(_ValidityPeriod);
    }

    #endregion

    #region Instance Members

    public override SmsVerificationCodeHasExpired CreateBusinessRuleViolationDomainEvent(UserRegistration merchant)
    {
        return new SmsVerificationCodeHasExpired(merchant, this);
    }

    public override bool IsBroken()
    {
        return _IsValid;
    }

    #endregion
}