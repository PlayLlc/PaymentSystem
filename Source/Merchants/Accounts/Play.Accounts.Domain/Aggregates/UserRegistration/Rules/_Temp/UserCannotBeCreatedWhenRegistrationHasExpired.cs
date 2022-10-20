using Play.Domain.Aggregates;
using Play.Domain.Events;
using Play.Globalization.Time;

namespace Play.Accounts.Domain.Aggregates;

internal class UserCannotBeCreatedWhenRegistrationHasExpired : BusinessRule<UserRegistration, string>
{
    #region Instance Values

    private readonly TimeSpan _ValidityPeriod = TimeSpan.FromDays(1);
    private readonly DateTimeUtc _RegisteredDate;

    public override string Message => "User cannot be created when registration has expired";

    #endregion

    #region Constructor

    public UserCannotBeCreatedWhenRegistrationHasExpired(DateTimeUtc registeredDate)
    {
        _RegisteredDate = registeredDate;
    }

    #endregion

    #region Instance Members

    public override bool IsBroken()
    {
        return (DateTimeUtc.Now - _RegisteredDate) > _ValidityPeriod;
    }

    public override UserRegistrationHasExpired CreateBusinessRuleViolationDomainEvent(UserRegistration aggregate)
    {
        return new UserRegistrationHasExpired(aggregate, this);
    }

    #endregion
}