using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Aggregates;
using Play.Globalization.Time;

namespace Play.Accounts.Domain.Aggregates;

internal class UserRegistrationMustNotExpire : BusinessRule<UserRegistration, string>
{
    #region Instance Values

    private readonly TimeSpan _ValidityPeriod = new(7, 0, 0, 0);
    private readonly bool _IsValid;

    public override string Message => "User cannot be created when registration has expired";

    #endregion

    #region Constructor

    public UserRegistrationMustNotExpire(UserRegistrationStatus status, DateTimeUtc registeredDate)
    {
        if (status == UserRegistrationStatuses.Expired)
        {
            _IsValid = false;

            return;
        }

        if ((DateTimeUtc.Now - registeredDate) > _ValidityPeriod)
        {
            _IsValid = false;

            return;
        }

        _IsValid = true;
    }

    #endregion

    #region Instance Members

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    public override UserRegistrationHasExpired CreateBusinessRuleViolationDomainEvent(UserRegistration aggregate)
    {
        return new UserRegistrationHasExpired(aggregate, this);
    }

    #endregion
}