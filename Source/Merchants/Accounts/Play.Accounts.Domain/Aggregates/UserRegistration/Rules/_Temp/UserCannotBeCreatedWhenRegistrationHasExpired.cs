using Play.Domain.Aggregates;
using Play.Globalization.Time;

namespace Play.Accounts.Domain.Aggregates;

internal class UserCannotBeCreatedWhenRegistrationHasExpired : IBusinessRule
{
    #region Instance Values

    private readonly TimeSpan _ValidityPeriod = TimeSpan.FromDays(1);
    private readonly DateTimeUtc _RegisteredDate;

    public string Message => "User cannot be created when registration has expired";

    #endregion

    #region Constructor

    public UserCannotBeCreatedWhenRegistrationHasExpired(DateTimeUtc registeredDate)
    {
        _RegisteredDate = registeredDate;
    }

    #endregion

    #region Instance Members

    public bool IsBroken()
    {
        return (DateTimeUtc.Now - _RegisteredDate) > _ValidityPeriod;
    }

    #endregion
}