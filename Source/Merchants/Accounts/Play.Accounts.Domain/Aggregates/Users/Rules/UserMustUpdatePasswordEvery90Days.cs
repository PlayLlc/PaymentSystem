using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Services;
using Play.Domain.Aggregates;
using Play.Globalization.Time;

namespace Play.Accounts.Domain.Aggregates;

internal class UserMustUpdatePasswordEvery90Days : BusinessRule<User, string>
{
    #region Instance Values

    private readonly bool _IsValid;
    private readonly TimeSpan _ValidityPeriod = new(90);
    public override string Message => "The user must update their password every 90 days";

    #endregion

    #region Constructor

    internal UserMustUpdatePasswordEvery90Days(Password password)
    {
        _IsValid = !password.IsExpired(_ValidityPeriod);
    }

    #endregion

    #region Instance Members

    public override UserMustUpdatePassword CreateBusinessRuleViolationDomainEvent(User aggregate)
    {
        return new UserMustUpdatePassword(aggregate, this);
    }

    public override bool IsBroken()
    {
        return _IsValid;
    }

    #endregion
}