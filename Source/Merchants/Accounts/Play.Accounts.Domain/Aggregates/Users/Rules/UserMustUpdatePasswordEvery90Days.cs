using Play.Accounts.Domain.Entities;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

internal class UserMustUpdatePasswordEvery90Days : BusinessRule<User, string>
{
    #region Instance Values

    private readonly bool _IsValid;
    private readonly TimeSpan _ValidityPeriod = new(90, 0, 0, 0);
    public override string Message => "The login attempt has failed because the user's password has expired";

    #endregion

    #region Constructor

    internal UserMustUpdatePasswordEvery90Days(Password password)
    {
        _IsValid = !password.IsExpired(_ValidityPeriod);
    }

    #endregion

    #region Instance Members

    public override UserMustUpdatePassword CreateBusinessRuleViolationDomainEvent(User merchant)
    {
        return new UserMustUpdatePassword(merchant, this);
    }

    public override bool IsBroken()
    {
        return _IsValid;
    }

    #endregion
}